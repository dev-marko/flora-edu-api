using System.Security.Claims;
using AutoMapper;
using FloraEdu.Application.Authentication.Interfaces;
using FloraEdu.Application.Services.Interfaces;
using FloraEdu.Domain.DataTransferObjects.Plant;
using FloraEdu.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FloraEdu.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class PlantsController : ControllerBase
{
    private readonly IPlantService _plantService;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public PlantsController(IPlantService plantService, IUserService userService, IMapper mapper)
    {
        _plantService = plantService;
        _userService = userService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IResult> GetPlants([FromQuery] PlantsRequestDto requestDto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        User? user = null;

        if (userId is not null)
        {
            user = await _userService.FindByIdAsync(Guid.Parse(userId));
        }

        var plants = await _plantService.GetPlantsQuery(requestDto.Page, requestDto.Size, requestDto.Type, user);

        return Results.Ok(plants);
    }

    [HttpGet("{plantId:guid}")]
    public async Task<IResult> GetPlantById(Guid plantId)
    {
        var plant = await _plantService.GetPlantById(plantId);
        if (plant is null) return Results.NotFound($"Plant with ID: {plantId} not found.");
        var mappedPlant = _mapper.Map<PlantDto>(plant);

        return Results.Ok(mappedPlant);
    }
}
