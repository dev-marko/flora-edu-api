using FloraEdu.Domain.DataTransferObjects;
using FloraEdu.Domain.DataTransferObjects.Article;
using FloraEdu.Domain.Entities;

namespace FloraEdu.Application.Services.Interfaces;

public interface IBlogService
{
    Task<PagedList<ArticleDto>> GetArticlesQuery(int page = 1, int pageSize = 10, string? searchTerm = null, User? user = null);
}
