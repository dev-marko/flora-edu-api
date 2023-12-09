using AutoMapper;
using FloraEdu.Application.Services.Interfaces;
using FloraEdu.Domain.DataTransferObjects;
using FloraEdu.Domain.DataTransferObjects.Article;
using FloraEdu.Domain.Entities;
using FloraEdu.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FloraEdu.Application.Services.Implementations;

public class BlogService : BaseService<Article>, IBlogService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ILogger<Article> _logger;

    public BlogService(ApplicationDbContext dbContext, ILogger<Article> logger, IMapper mapper) : base(dbContext,
        logger)
    {
        _dbContext = dbContext;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<Article?> GetArticleById(Guid id)
    {
        var entity = await _dbContext.Set<Article>().Include(p => p.Author).FirstOrDefaultAsync(p => p.Id == id);

        if (entity is null) _logger.LogError("Entity with ID: {id} not found", id);

        return entity;
    }

    public async Task<PagedList<ArticleDto>> GetArticlesQuery(int page = 1, int pageSize = 10,
        string? searchTerm = null,
        User? user = null)
    {
        IQueryable<Article> articles = _dbContext.Set<Article>().Include(a => a.Author).Include(a => a.Likes)
            .Include(a => a.Bookmarks);

        if (!string.IsNullOrEmpty(searchTerm))
        {
            articles = articles.Where(a =>
                a.Title.Contains(searchTerm) ||
                (a.Author.FirstName != null && a.Author.FirstName.Contains(searchTerm)) ||
                (a.Author.LastName != null && a.Author.LastName.Contains(searchTerm)) ||
                a.ShortDescription.Contains(searchTerm));
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
                CreatedAt = article.CreatedAt,
                LastModified = article.LastModified
            });

        var articleDtosPagedList = await PagedList<ArticleDto>.CreateAsync(articleDtos, page, pageSize);

        return articleDtosPagedList;
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
