using FloraEdu.Domain.DataTransferObjects;
using FloraEdu.Domain.DataTransferObjects.Plant;
using FloraEdu.Domain.Entities;
using FloraEdu.Domain.Enumerations;

namespace FloraEdu.Application.Services.Interfaces;

public interface IPlantService : IService<Plant>
{
    Task<Plant?> GetPlantById(Guid id);
    Task<PagedList<PlantCardDto>> GetPlantsQuery(int page = 1, int pageSize = 10, PlantType type = PlantType.Unknown, User? user = null);
    Task<List<PlantDto>> QueryPlantByName(string name);
    Task<List<PlantDto>> GetAllByType(PlantType type);
}
