using AutoMapper;
using FloraEdu.Application.Services.Interfaces;
using FloraEdu.Domain.DataTransferObjects;
using FloraEdu.Domain.DataTransferObjects.Article;
using FloraEdu.Domain.DataTransferObjects.Plant;
using FloraEdu.Domain.Entities;
using FloraEdu.Domain.Enumerations;
using FloraEdu.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FloraEdu.Application.Services.Implementations;

public class UserFeaturesService : IUserFeaturesService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public UserFeaturesService(ApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<User?> GetUser(string userId)
    {
        var user = await _dbContext.Set<User>()
            .Include(u => u.BookmarkedArticles)
            .ThenInclude(a => a.Author)
            .Include(u => u.BookmarkedArticles)
            .ThenInclude(a => a.Likes)
            .Include(u => u.BookmarkedPlants)
            .ThenInclude(p => p.Likes)
            .FirstOrDefaultAsync(u => u.Id == userId);

        return user;
    }

    public PagedList<PlantCardDto> GetBookmarkedPlants(User user, int page = 1, int pageSize = 8,
        PlantType type = PlantType.Unknown, string? searchTerm = null)
    {
        var plants = user.BookmarkedPlants;

        if (type != PlantType.Unknown)
        {
            plants = plants.Where(p => p.Type == type).ToList();
        }

        if (!string.IsNullOrEmpty(searchTerm))
        {
            var normalizedSearchTerm = searchTerm.ToLower();
            plants = plants.Where(p =>
                p.Name.ToLower().Contains(normalizedSearchTerm) ||
                (p.Author.FirstName != null && p.Author.FirstName.Contains(searchTerm)) ||
                (p.Author.LastName != null && p.Author.LastName.Contains(searchTerm)) ||
                p.Description.Contains(searchTerm)).ToList();
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
            .OrderBy(p => p.Name)
            .ToList();
        var plantCardsPagedList = PagedList<PlantCardDto>.Create(plantCards, page, pageSize);

        return plantCardsPagedList;
    }

    public PagedList<ArticleDto> GetBookmarkedArticles(User user, int page = 1, int pageSize = 10,
        string? searchTerm = null)
    {
        var articles = user.BookmarkedArticles;

        if (!string.IsNullOrEmpty(searchTerm))
        {
            articles = articles.Where(a =>
                a.Title.Contains(searchTerm) ||
                (a.Author.FirstName != null && a.Author.FirstName.Contains(searchTerm)) ||
                (a.Author.LastName != null && a.Author.LastName.Contains(searchTerm)) ||
                a.ShortDescription.Contains(searchTerm)).ToList();
        }

        var articleDtos = articles
            .Select(article => new ArticleDto
            {
                Id = article.Id,
                Title = article.Title,
                Subtitle = article.Subtitle,
                ShortDescription = article.ShortDescription,
                HeaderImageUrl = article.HeaderImageUrl,
                Content = article.Content,
                Author = _mapper.Map<AuthorDto>(article.Author),
                IsBookmarked = CheckIfArticleIsBookmarked(article, user),
                IsLiked = CheckIfArticleIsLiked(article, user),
                LikeCount = article.Likes.Count,
                CreatedAt = article.CreatedAt,
                LastModified = article.LastModified
            }).ToList();

        var articleDtosPagedList = PagedList<ArticleDto>.Create(articleDtos, page, pageSize);

        return articleDtosPagedList;
    }

    private static bool CheckIfPlantIsBookmarked(Plant plant, User? user)
    {
        return user is not null && plant.Bookmarks.Contains(user);
    }

    private static bool CheckIfPlantIsLiked(Plant plant, User? user)
    {
        return user is not null && plant.Likes.Contains(user);
    }

    private static bool CheckIfArticleIsBookmarked(Article article, User? user)
    {
        return user is not null && article.Bookmarks.Contains(user);
    }

    private static bool CheckIfArticleIsLiked(Article article, User? user)
    {
        return user is not null && article.Likes.Contains(user);
    }
}
