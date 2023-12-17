using System.Security.Claims;
using AutoMapper;
using FloraEdu.Application.Authentication.Interfaces;
using FloraEdu.Application.Services.Interfaces;
using FloraEdu.Domain.Authorization;
using FloraEdu.Domain.DataTransferObjects.Plant;
using FloraEdu.Domain.Enumerations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FloraEdu.Web.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(AuthorizationPolicies.AdminOrSpecialist)]
public class DashboardController : ControllerBase
{
    private readonly IBlogService _blogService;
    private readonly IPlantService _plantService;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public DashboardController(IPlantService plantService, IUserService userService, IMapper mapper,
        IBlogService blogService)
    {
        _blogService = blogService;
        _plantService = plantService;
        _userService = userService;
        _mapper = mapper;
    }

    [HttpGet("plants")]
    public async Task<IResult> GetPlants([FromQuery] PlantsRequestDto requestDto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId is null)
        {
            return Results.Unauthorized();
        }

        var user = await _userService.FindByIdAsync(Guid.Parse(userId));
        var plants = await _plantService.GetPlantsByCreator(user, requestDto.Page, requestDto.Size);

        return Results.Ok(plants);
    }

    [HttpGet("plants/{plantId:guid}")]
    public async Task<IResult> GetPlantById(Guid plantId)
    {
        var plant = await _plantService.GetPlantById(plantId);
        if (plant is null) return Results.NotFound($"Plant with ID: {plantId} not found.");
        var mappedPlant = _mapper.Map<PlantDto>(plant);

        return Results.Ok(mappedPlant);
    }

    [HttpGet("plant-analytics")]
    public async Task<IResult> GetPlantAnalyticsByCreator()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId is null) return Results.Unauthorized();
        var user = await _userService.FindByIdAsync(Guid.Parse(userId));

        var mostPopularByLikes = await _plantService.GetMostPopularPlantByLikes(userId);
        var mostPopularByBookmarks = await _plantService.GetMostPopularPlantByBookmarks(userId);
        var mostPopularByNumberOfComments = await _plantService.GetMostInteractedPlantByComments(userId);
        var mostPopularByUniqueVisitors = await _plantService.GetMostPopularPlantByUniqueVisitors(userId);

        var plantAnalytics = new PlantAnalytics
        {
            MostPopularByLikes = mostPopularByLikes.Name ?? "",
            MostPopularByBookmarks = mostPopularByBookmarks.Name ?? "",
            MostPopularByNumberOfComments = mostPopularByNumberOfComments.Name ?? "",
            MostPopularByUniqueVisitors = mostPopularByUniqueVisitors.Name ?? ""
        };

        return Results.Ok(plantAnalytics);
    }

    [HttpPut("plants")]
    public async Task<IResult> UpdatePlant([FromBody] PlantEditDto plantDto)
    {
        var didParse = Enum.TryParse<PlantType>(plantDto.Type, out var plantType);
        var plantToEdit = await _plantService.GetById(plantDto.Id);

        plantToEdit.Name = plantDto.Name;
        plantToEdit.Description = plantDto.Description;
        plantToEdit.Type = didParse ? plantType : PlantType.Unknown;
        plantToEdit.Predispositions = plantDto.Predispositions;
        plantToEdit.Maintenance = plantDto.Maintenance;
        plantToEdit.Planting = plantDto.Planting;
        plantToEdit.LastModified = DateTime.UtcNow;

        var res = await _plantService.Update(plantToEdit);

        return Results.Ok(res);
    }

    [HttpDelete("plants")]
    public async Task<IResult> DeletePlant([FromBody] string plantId)
    {
        var deleted = await _plantService.Delete(Guid.Parse(plantId));

        return deleted ? Results.Ok() : Results.BadRequest();
    }
}
