using FloraEdu.Domain.DataTransferObjects;
using FloraEdu.Domain.DataTransferObjects.Plant;
using FloraEdu.Domain.Entities;
using FloraEdu.Domain.Enumerations;

namespace FloraEdu.Application.Services.Interfaces;

public interface IPlantService : IService<Plant>
{
    // General Service Methods
    Task<Plant?> GetPlantById(Guid id);
    Task<PagedList<PlantCardDto>> GetPlantsByCreator(User user, int page = 1, int pageSize = 8);

    Task<PagedList<PlantCardDto>> GetPlantsQuery(int page = 1, int pageSize = 8, PlantType type = PlantType.Unknown,
        string? searchTerm = null,
        User? user = null);

    Task<List<PlantCardDto>> GetMostPopularPlantsGlobally(int take = 3, User? user = null);
    Task<bool> LikePlant(Plant plant, User user);
    Task<bool> BookmarkPlant(Plant plant, User user);
    void RegisterUniqueVisitor(Guid uuaid, Guid plantId, string? userId);

    // Specialist Analytics
    Task<(string, int)> GetMostPopularPlantByLikes(string userId);
    Task<(string, int)> GetMostPopularPlantByBookmarks(string userId);
    Task<(string, int)> GetMostInteractedPlantByComments(string userId);
    Task<(string, int)> GetMostPopularPlantByUniqueVisitors(string userId);

    // Plant Comments
    Task<PlantComment?> GetPlantCommentById(Guid plantCommentId);
    Task<bool> AddNewComment(User user, Guid plantId, string content);
    Task<bool> LikePlantComment(PlantComment plantComment, User user);
}
