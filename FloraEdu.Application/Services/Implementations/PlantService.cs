using AutoMapper;
using FloraEdu.Application.Authentication.Interfaces;
using FloraEdu.Application.Services.Interfaces;
using FloraEdu.Domain.DataTransferObjects;
using FloraEdu.Domain.DataTransferObjects.Plant;
using FloraEdu.Domain.Entities;
using FloraEdu.Domain.Enumerations;
using FloraEdu.Domain.Exceptions;
using FloraEdu.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FloraEdu.Application.Services.Implementations;

public class PlantService : BaseService<Plant>, IPlantService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;
    private readonly ILogger<Plant> _logger;

    public PlantService(ApplicationDbContext dbContext, ILogger<Plant> logger, IMapper mapper, IUserService userService)
        : base(dbContext, logger)
    {
        _dbContext = dbContext;
        _logger = logger;
        _mapper = mapper;
        _userService = userService;
    }

    public async Task<Plant?> GetPlantById(Guid id)
    {
        var entity = await _dbContext.Set<Plant>().Include(p => p.Author).FirstOrDefaultAsync(p => p.Id == id);

        if (entity is null) _logger.LogError("Entity with ID: {id} not found", id);

        return entity;
    }

    public async Task<PagedList<PlantCardDto>> GetPlantsQuery(int page = 1, int pageSize = 10,
        PlantType type = PlantType.Unknown, User? user = null)
    {
        var plants = _dbContext.Set<Plant>().Include(p => p.Likes).Include(p => p.Bookmarks);

        if (type != PlantType.Unknown)
        {
            plants = plants.Where(p => p.Type == type).Include(p => p.Likes).Include(p => p.Bookmarks);
        }

        var plantCards = plants
            .Select(plant => new PlantCardDto
            {
                Id = plant.Id,
                Name = plant.Name,
                Type = plant.Type.ToString(),
                Description = plant.Description,
                CreatedAt = plant.CreatedAt,
                LastModified = plant.LastModified,
                IsBookmarked = CheckIfPlantIsBookmarked(plant, user),
                IsLiked = CheckIfPlantIsLiked(plant, user)
            })
            .OrderBy(p => p.Name);
        var plantCardsPagedList = await PagedList<PlantCardDto>.CreateAsync(plantCards, page, pageSize);

        return plantCardsPagedList;
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

    private static bool CheckIfPlantIsBookmarked(Plant plant, User? user)
    {
        return user is not null && plant.Bookmarks.Contains(user);
    }

    private static bool CheckIfPlantIsLiked(Plant plant, User? user)
    {
        return user is not null && plant.Likes.Contains(user);
    }
}
