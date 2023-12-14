using AutoMapper;
using FloraEdu.Application.Services.Interfaces;
using FloraEdu.Domain.DataTransferObjects;
using FloraEdu.Domain.DataTransferObjects.Plant;
using FloraEdu.Domain.Entities;
using FloraEdu.Domain.Enumerations;
using FloraEdu.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FloraEdu.Application.Services.Implementations;

public class PlantService : BaseService<Plant>, IPlantService
{
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

    public async Task<PagedList<PlantCardDto>> GetPlantsByCreator(User user, int page = 1, int pageSize = 10)
    {
        var plants = _dbContext.Set<Plant>()
            .Include(p => p.Author)
            .Where(p => p.Author != null && p.Author.Id == user.Id);

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

    public async Task<PagedList<PlantCardDto>> GetPlantsQuery(int page = 1, int pageSize = 10,
        PlantType type = PlantType.Unknown, User? user = null)
    {
        var plants = _dbContext.Set<Plant>().Include(p => p.Likes).Include(p => p.Bookmarks);

        if (type != PlantType.Unknown)
        {
            plants = plants.Where(p => p.Type == type).Include(p => p.Likes).Include(p => p.Bookmarks);
        }

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
            .OrderBy(p => p.Name);
        var plantCardsPagedList = await PagedList<PlantCardDto>.CreateAsync(plantCards, page, pageSize);

        return plantCardsPagedList;
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
