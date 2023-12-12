using FloraEdu.Domain.DataTransferObjects;
using FloraEdu.Domain.DataTransferObjects.Plant;
using FloraEdu.Domain.Entities;
using FloraEdu.Domain.Enumerations;

namespace FloraEdu.Application.Services.Interfaces;

public interface IPlantService : IService<Plant>
{
    Task<Plant?> GetPlantById(Guid id);
    Task<PagedList<PlantCardDto>> GetPlantsByCreator(User user, int page = 1, int pageSize = 10);

    Task<PagedList<PlantCardDto>> GetPlantsQuery(int page = 1, int pageSize = 10, PlantType type = PlantType.Unknown,
        User? user = null);

    Task<List<PlantDto>> QueryPlantByName(string name);
    Task<List<PlantDto>> GetAllByType(PlantType type);

    // Plant Comments
    Task<PlantComment?> GetPlantCommentById(Guid plantCommentId);
    Task<bool> AddNewComment(User user, Guid plantId, string content);
    Task<bool> LikePlantComment(PlantComment plantComment, User user);
    Task<bool> CheckIfPlantCommentIsLiked(Guid plantCommentId, User user);
    Task<bool> UnlikePlantComment(PlantComment plantComment, User user);
}
