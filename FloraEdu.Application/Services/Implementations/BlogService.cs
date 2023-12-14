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
        var entity = await _dbContext.Set<Article>()
            .Include(a => a.Author)
            .Include(a => a.Likes)
            .Include(a => a.Bookmarks)
            .Include(a => a.Comments)
            .ThenInclude(ac => ac.User)
            .Include(a => a.Comments.OrderByDescending(c => c.CreatedAt))
            .ThenInclude(ac => ac.Likes)
            .FirstOrDefaultAsync(a => a.Id == id);

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
                LikeCount = article.Likes.Count,
                CreatedAt = article.CreatedAt,
                LastModified = article.LastModified
            });

        var articleDtosPagedList = await PagedList<ArticleDto>.CreateAsync(articleDtos, page, pageSize);

        return articleDtosPagedList;
    }

    public async Task<ArticleComment?> GetArticleCommentById(Guid articleCommentId)
    {
        var articleComment = await _dbContext.Set<ArticleComment>()
            .Include(pc => pc.Likes)
            .FirstOrDefaultAsync(p => p.Id == articleCommentId);

        if (articleComment is null) _logger.LogError("ArticleComment with ID: {id} not found", articleCommentId);

        return articleComment;
    }

    public async Task<bool> AddNewComment(User user, Guid articleId, string content)
    {
        var article = await GetArticleById(articleId);

        var newComment = new ArticleComment
        {
            Article = article!,
            UserId = user.Id,
            User = user,
            Content = content,
        };

        article!.Comments.Add(newComment);

        var res = await _dbContext.SaveChangesAsync();

        return res > 0;
    }

    public async Task<bool> LikeArticleComment(ArticleComment articleComment, User user)
    {
        articleComment.Likes.Add(user);
        var res = await _dbContext.SaveChangesAsync();

        return res > 0;
    }

    public async Task<bool> UnlikeArticleComment(ArticleComment articleComment, User user)
    {
        articleComment.Likes.Remove(user);
        var res = await _dbContext.SaveChangesAsync();

        return res > 0;
    }

    public async Task<bool> LikeArticle(Article article, User user)
    {
        article.Likes.Add(user);
        var res = await _dbContext.SaveChangesAsync();

        return res > 0;
    }

    public async Task<bool> UnlikeArticle(Article article, User user)
    {
        article.Likes.Remove(user);
        var res = await _dbContext.SaveChangesAsync();

        return res > 0;
    }

    public async Task<bool> BookmarkArticle(Article article, User user)
    {
        if (!article.Bookmarks.Contains(user))
        {
            article.Bookmarks.Add(user);
        }
        else
        {
            article.Bookmarks.Remove(user);
        }

        var res = await _dbContext.SaveChangesAsync();

        return res > 0;
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
