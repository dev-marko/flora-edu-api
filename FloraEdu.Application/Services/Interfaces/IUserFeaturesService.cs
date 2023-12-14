using FloraEdu.Domain.DataTransferObjects;
using FloraEdu.Domain.DataTransferObjects.Article;
using FloraEdu.Domain.DataTransferObjects.Plant;
using FloraEdu.Domain.Entities;
using FloraEdu.Domain.Enumerations;

namespace FloraEdu.Application.Services.Interfaces;

public interface IUserFeaturesService
{
    Task<User?> GetUser(string userId);

    PagedList<PlantCardDto> GetBookmarkedPlants(User user, int page = 1, int pageSize = 10,
        PlantType type = PlantType.Unknown);

    PagedList<ArticleDto> GetBookmarkedArticles(User user, int page = 1, int pageSize = 10,
        string? searchTerm = null);
}
