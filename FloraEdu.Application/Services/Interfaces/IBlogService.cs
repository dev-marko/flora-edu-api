using FloraEdu.Domain.DataTransferObjects;
using FloraEdu.Domain.DataTransferObjects.Analytics;
using FloraEdu.Domain.DataTransferObjects.Article;
using FloraEdu.Domain.Entities;

namespace FloraEdu.Application.Services.Interfaces;

public interface IBlogService : IService<Article>
{
    Task<Article?> GetArticleById(Guid id);
    Task<PagedList<ArticleDto>> GetArticlesByCreator(User user, int page = 1, int pageSize = 8);

    Task<PagedList<ArticleDto>> GetArticlesQuery(int page = 1, int pageSize = 10, string? searchTerm = null,
        User? user = null);

    Task<List<ArticleDto>> GetMostPopularArticlesGlobally(int take = 3, User? user = null);

    Task<bool> LikeArticle(Article article, User user);
    Task<bool> BookmarkArticle(Article article, User user);
    void RegisterUniqueVisitor(Guid uuaid, Guid articleId, string? userId);

    // Specialist Analytics
    Task<(string, int)> GetMostPopularArticleByLikes(string userId);
    Task<(string, int)> GetMostPopularArticleByBookmarks(string userId);
    Task<(string, int)> GetMostInteractedArticleByComments(string userId);
    Task<(string, int)> GetMostPopularArticleByUniqueVisitors(string userId);

    // Article Comments
    Task<ArticleComment?> GetArticleCommentById(Guid articleCommentId);
    Task<bool> AddNewComment(User user, Guid articleId, string content);
    Task<bool> LikeArticleComment(ArticleComment articleComment, User user);
    Task<List<LikesDataDto>> GetArticleLikesChartData(string userId);
    Task<List<BookmarksDataDto>> GetArticleBookmarksChartData(string userId);
}
