using AutoMapper;
using FloraEdu.Application.Services.Interfaces;
using FloraEdu.Domain.DataTransferObjects;
using FloraEdu.Domain.DataTransferObjects.Analytics;
using FloraEdu.Domain.DataTransferObjects.Plant;
using FloraEdu.Domain.Entities;
using FloraEdu.Domain.Enumerations;
using FloraEdu.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FloraEdu.Application.Services.Implementations;

public class PlantService : BaseService<Plant>, IPlantService
{
    private readonly object _registerUniqueUserLock = new();
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ILogger<Plant> _logger;

    public PlantService(ApplicationDbContext dbContext, ILogger<Plant> logger, IMapper mapper)
        : base(dbContext, logger)
    {
        _dbContext = dbContext;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<Plant?> GetPlantById(Guid id)
    {
        var entity = await _dbContext.Set<Plant>()
            .Include(p => p.Author)
            .Include(p => p.PlantImage)
            .Include(p => p.Likes)
            .Include(p => p.Bookmarks)
            .Include(p => p.Comments)
            .ThenInclude(pc => pc.User)
            .Include(p => p.Comments.OrderByDescending(c => c.CreatedAt))
            .ThenInclude(pc => pc.Likes)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (entity is null) _logger.LogError("Entity with ID: {id} not found", id);

        return entity;
    }

    public async Task<PagedList<PlantCardDto>> GetPlantsByCreator(User user, int page = 1, int pageSize = 8)
    {
        var plants = _dbContext.Set<Plant>()
            .Include(p => p.Author)
            .Where(p => p.Author.Id == user.Id);

        var plantCards = plants
            .Select(plant => new PlantCardDto
            {
                Id = plant.Id,
                Name = plant.Name,
                Type = plant.Type.ToString(),
                Description = plant.Description,
                CreatedAt = plant.CreatedAt,
                LastModified = plant.LastModified,
                IsBookmarked = false,
                IsLiked = false
            })
            .OrderByDescending(p => p.LastModified);

        var plantCardsPagedList = await PagedList<PlantCardDto>.CreateAsync(plantCards, page, pageSize);

        return plantCardsPagedList;
    }

    public async Task<PagedList<PlantCardDto>> GetPlantsQuery(int page = 1, int pageSize = 8,
        PlantType type = PlantType.Unknown, string? searchTerm = null, User? user = null)
    {
        IQueryable<Plant> plants = _dbContext.Set<Plant>().Include(p => p.Likes).Include(p => p.Bookmarks)
            .Include(p => p.PlantImage);

        if (type != PlantType.Unknown)
        {
            plants = plants.Where(p => p.Type == type).Include(p => p.Likes).Include(p => p.Bookmarks);
        }

        if (!string.IsNullOrEmpty(searchTerm))
        {
            var normalizedSearchTerm = searchTerm.ToLower();
            plants = plants.Where(p =>
                p.Name.ToLower().Contains(normalizedSearchTerm) ||
                (p.Author.FirstName != null && p.Author.FirstName.Contains(searchTerm)) ||
                (p.Author.LastName != null && p.Author.LastName.Contains(searchTerm)) ||
                p.Description.Contains(searchTerm));
        }

        var plantCards = plants
            .Select(plant => new PlantCardDto
            {
                Id = plant.Id,
                Name = plant.Name,
                Type = plant.Type.ToString(),
                Description = plant.Description,
                ThumbnailImageUrl = plant.PlantImage!.ThumbnailImageUrl,
                CreatedAt = plant.CreatedAt,
                LastModified = plant.LastModified,
                IsBookmarked = CheckIfPlantIsBookmarked(plant, user),
                IsLiked = CheckIfPlantIsLiked(plant, user),
                LikeCount = plant.Likes.Count
            })
            .OrderBy(p => p.Name);
        var plantCardsPagedList = await PagedList<PlantCardDto>.CreateAsync(plantCards, page, pageSize);

        return plantCardsPagedList;
    }

    public Task<List<PlantCardDto>> GetMostPopularPlantsGlobally(int take = 3, User? user = null)
    {
        var plants = _dbContext.Set<Plant>().Include(p => p.Likes).Include(p => p.Bookmarks);

        var plantCards = plants
            .Select(plant => new PlantCardDto
            {
                Id = plant.Id,
                Name = plant.Name,
                Type = plant.Type.ToString(),
                Description = plant.Description,
                CreatedAt = plant.CreatedAt,
                LastModified = plant.LastModified,
                IsBookmarked = CheckIfPlantIsBookmarked(plant, user),
                IsLiked = CheckIfPlantIsLiked(plant, user),
                LikeCount = plant.Likes.Count
            })
            .OrderByDescending(p => p.LikeCount)
            .Take(take)
            .ToListAsync();

        return plantCards;
    }

    public async Task<(string, int)> GetMostPopularPlantByLikes(string userId)
    {
        var plants = await _dbContext.Set<Plant>()
            .Include(p => p.Author)
            .Include(p => p.UniqueVisitors)
            .Include(p => p.Likes)
            .Include(p => p.Bookmarks)
            .Include(p => p.Comments)
            .ThenInclude(pc => pc.User)
            .Include(p => p.Comments.OrderByDescending(c => c.CreatedAt))
            .ThenInclude(pc => pc.Likes)
            .Where(a => a.Author.Id == userId)
            .ToListAsync();

        if (plants.Count == 0)
        {
            return (string.Empty, 0);
        }

        var plant = plants
            .OrderByDescending(p => p.Likes.Count)
            .First();

        var mapped = _mapper.Map<PlantDto>(plant);

        return (mapped.Name!, mapped.LikeCount);
    }

    public async Task<(string, int)> GetMostPopularPlantByBookmarks(string userId)
    {
        var plants = await _dbContext.Set<Plant>()
            .Include(p => p.Author)
            .Include(p => p.UniqueVisitors)
            .Include(p => p.Likes)
            .Include(p => p.Bookmarks)
            .Include(p => p.Comments)
            .ThenInclude(pc => pc.User)
            .Include(p => p.Comments.OrderByDescending(c => c.CreatedAt))
            .ThenInclude(pc => pc.Likes)
            .Where(a => a.Author.Id == userId)
            .ToListAsync();

        if (plants.Count == 0)
        {
            return (string.Empty, 0);
        }

        var plant = plants
            .OrderByDescending(p => p.Bookmarks.Count)
            .First();

        var mapped = _mapper.Map<PlantDto>(plant);

        return (mapped.Name!, mapped.BookmarkCount);
    }

    public async Task<(string, int)> GetMostInteractedPlantByComments(string userId)
    {
        var plants = await _dbContext.Set<Plant>()
            .Include(p => p.Author)
            .Include(p => p.UniqueVisitors)
            .Include(p => p.Likes)
            .Include(p => p.Bookmarks)
            .Include(p => p.Comments)
            .ThenInclude(pc => pc.User)
            .Include(p => p.Comments.OrderByDescending(c => c.CreatedAt))
            .ThenInclude(pc => pc.Likes)
            .Where(a => a.Author.Id == userId)
            .ToListAsync();

        if (plants.Count == 0)
        {
            return (string.Empty, 0);
        }

        var plant = plants
            .OrderByDescending(p => p.Comments.Count)
            .First();

        var mapped = _mapper.Map<PlantDto>(plant);

        return (mapped.Name!, mapped.CommentCount);
    }

    public async Task<(string, int)> GetMostPopularPlantByUniqueVisitors(string userId)
    {
        var plants = await _dbContext.Set<Plant>()
            .Include(p => p.Author)
            .Include(p => p.UniqueVisitors)
            .Include(p => p.Likes)
            .Include(p => p.Bookmarks)
            .Include(p => p.Comments)
            .ThenInclude(pc => pc.User)
            .Include(p => p.Comments.OrderByDescending(c => c.CreatedAt))
            .ThenInclude(pc => pc.Likes)
            .Where(a => a.Author.Id == userId)
            .ToListAsync();

        if (plants.Count == 0)
        {
            return (string.Empty, 0);
        }

        var plant = plants
            .OrderByDescending(p => p.UniqueVisitors.Count)
            .First();

        var mapped = _mapper.Map<PlantDto>(plant);

        return (mapped.Name!, mapped.VisitorsCount);
    }

    public async Task<List<LikesDataDto>> GetPlantLikesChartData(string userId)
    {
        var plants = await _dbContext.Set<Plant>()
            .Include(p => p.Author)
            .Include(p => p.Likes)
            .Where(a => a.Author.Id == userId)
            .ToListAsync();

        var plantLikesChartData = plants.Select(p => new LikesDataDto
            {
                EntityId = p.Id,
                Name = p.Name,
                LikesCount = p.Likes.Count
            })
            .OrderByDescending(p => p.LikesCount)
            .ToList();

        return plantLikesChartData;
    }

    public async Task<List<BookmarksDataDto>> GetPlantBookmarksChartData(string userId)
    {
        var plants = await _dbContext.Set<Plant>()
            .Include(p => p.Author)
            .Include(p => p.Bookmarks)
            .Where(a => a.Author.Id == userId)
            .ToListAsync();

        var plantBookmarksChartData = plants.Select(p => new BookmarksDataDto
            {
                EntityId = p.Id,
                Name = p.Name,
                BookmarksCount = p.Bookmarks.Count
            })
            .OrderByDescending(p => p.BookmarksCount)
            .ToList();

        return plantBookmarksChartData;
    }

    public async Task<bool> BookmarkPlant(Plant plant, User user)
    {
        if (!plant.Bookmarks.Contains(user))
        {
            plant.Bookmarks.Add(user);
        }
        else
        {
            plant.Bookmarks.Remove(user);
        }

        var res = await _dbContext.SaveChangesAsync();

        return res > 0;
    }

    public async Task<PlantComment?> GetPlantCommentById(Guid plantCommentId)
    {
        var plantComment = await _dbContext.Set<PlantComment>()
            .Include(pc => pc.Likes)
            .FirstOrDefaultAsync(p => p.Id == plantCommentId);

        if (plantComment is null) _logger.LogError("PlantComment with ID: {id} not found", plantCommentId);

        return plantComment;
    }

    public async Task<bool> AddNewComment(User user, Guid plantId, string content)
    {
        var plant = await GetPlantById(plantId);

        var newComment = new PlantComment
        {
            Plant = plant!,
            UserId = user.Id,
            User = user,
            Content = content,
        };

        plant!.Comments.Add(newComment);

        var res = await _dbContext.SaveChangesAsync();

        return res > 0;
    }

    public async Task<bool> LikePlantComment(PlantComment plantComment, User user)
    {
        if (!plantComment.Likes.Contains(user))
        {
            plantComment.Likes.Add(user);
        }
        else
        {
            plantComment.Likes.Remove(user);
        }

        var res = await _dbContext.SaveChangesAsync();

        return res > 0;
    }

    public void RegisterUniqueVisitor(Guid uuaid, Guid plantId, string? userId)
    {
        lock (_registerUniqueUserLock)
        {
            var plantVisitors = _dbContext.Set<UniquePlantVisitor>();
            var userAlreadyRegistered = plantVisitors.Any(visitor =>
                (visitor.UserId == userId || visitor.UUAID == uuaid) && visitor.PlantId == plantId);

            if (userAlreadyRegistered)
            {
                return;
            }

            var newUniqueVisitor = new UniquePlantVisitor
            {
                UUAID = uuaid,
                PlantId = plantId,
                UserId = userId,
                IsAnonymous = userId is null
            };

            plantVisitors.Add(newUniqueVisitor);

            _dbContext.SaveChanges();
        }
    }

    public async Task<bool> CheckIfPlantCommentIsLiked(Guid plantCommentId, User user)
    {
        var plantComment = await GetPlantCommentById(plantCommentId);

        return plantComment != null && plantComment.Likes.Contains(user);
    }


    public async Task<List<PlantDto>> QueryPlantByName(string name)
    {
        var plants = _dbContext.Set<Plant>();
        var queryResult = await plants.Where(plant => plant.Name.Contains(name)).ToListAsync();
        var mappedResult = _mapper.Map<List<PlantDto>>(queryResult);

        return mappedResult;
    }

    public async Task<List<PlantDto>> GetAllByType(PlantType type)
    {
        var plants = _dbContext.Set<Plant>();
        var queryResult = await plants.Where(plant => plant.Type == type).ToListAsync();
        var mappedResult = _mapper.Map<List<PlantDto>>(queryResult);

        return mappedResult;
    }

    public async Task<bool> LikePlant(Plant plant, User user)
    {
        if (!plant.Likes.Contains(user))
        {
            plant.Likes.Add(user);
        }
        else
        {
            plant.Likes.Remove(user);
        }

        var res = await _dbContext.SaveChangesAsync();

        return res > 0;
    }

    private static bool CheckIfPlantIsBookmarked(Plant plant, User? user)
    {
        return user is not null && plant.Bookmarks.Contains(user);
    }

    private static bool CheckIfPlantIsLiked(Plant plant, User? user)
    {
        return user is not null && plant.Likes.Contains(user);
    }
}
