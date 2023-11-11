using AutoMapper;
using FloraEdu.Application.Services.Interfaces;
using FloraEdu.Domain.DataTransferObjects.Plant;
using FloraEdu.Domain.Entities;
using FloraEdu.Domain.Enumerations;
using FloraEdu.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FloraEdu.Application.Services.Implementations;

public class PlantService : BaseService<Plant>, IPlantService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public PlantService(ApplicationDbContext dbContext, ILogger logger, IMapper mapper) : base(dbContext, logger)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<List<PlantDto>> QueryPlantByName(string name)
    {
        var plants = _dbContext.Set<Plant>();
        var queryResult = await plants.Where(plant => plant.Name.Contains(name)).ToListAsync();
        var mappedResult = _mapper.Map<List<PlantDto>>(queryResult);

        return mappedResult;
    }

    public async Task<List<PlantDto>> GetAllByType(PlantType type)
    {
        var plants = _dbContext.Set<Plant>();
        var queryResult = await plants.Where(plant => plant.Type == type).ToListAsync();
        var mappedResult = _mapper.Map<List<PlantDto>>(queryResult);

        return mappedResult;
    }
}
