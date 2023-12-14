using FloraEdu.Domain.DataTransferObjects;
using FloraEdu.Domain.DataTransferObjects.Article;
using FloraEdu.Domain.Entities;

namespace FloraEdu.Application.Services.Interfaces;

public interface IBlogService
{
    Task<Article?> GetArticleById(Guid id);

    Task<PagedList<ArticleDto>> GetArticlesQuery(int page = 1, int pageSize = 10, string? searchTerm = null,
        User? user = null);

    // Article Comments
    Task<ArticleComment?> GetArticleCommentById(Guid articleCommentId);
    Task<bool> AddNewComment(User user, Guid articleId, string content);
    Task<bool> LikeArticle(Article article, User user);
    Task<bool> LikeArticleComment(ArticleComment articleComment, User user);
    Task<bool> BookmarkArticle(Article article, User user);
}
