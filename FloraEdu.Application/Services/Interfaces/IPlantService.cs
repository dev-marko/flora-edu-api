using FloraEdu.Domain.DataTransferObjects.Plant;
using FloraEdu.Domain.Entities;
using FloraEdu.Domain.Enumerations;

namespace FloraEdu.Application.Services.Interfaces;

public interface IPlantService : IService<Plant>
{
    Task<List<PlantDto>> QueryPlantByName(string name);
    Task<List<PlantDto>> GetAllByType(PlantType type);
}
