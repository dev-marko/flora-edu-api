using System.Security.Claims;
using AutoMapper;
using FloraEdu.Application.Authentication.Interfaces;
using FloraEdu.Application.Services.Interfaces;
using FloraEdu.Domain.Authorization;
using FloraEdu.Domain.DataTransferObjects;
using FloraEdu.Domain.DataTransferObjects.Plant;
using FloraEdu.Domain.Entities;
using FloraEdu.Domain.Enumerations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace FloraEdu.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class PlantsController : ControllerBase
{
    private readonly IPlantService _plantService;
    private readonly IUserService _userService;
    private readonly IUserFeaturesService _userFeaturesService;
    private readonly IMapper _mapper;

    public PlantsController(IPlantService plantService, IUserService userService, IMapper mapper,
        IUserFeaturesService userFeaturesService)
    {
        _plantService = plantService;
        _userService = userService;
        _mapper = mapper;
        _userFeaturesService = userFeaturesService;
    }

    [HttpGet]
    public async Task<IResult> GetPlants([FromQuery] PlantsRequestDto requestDto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        User? user = null;

        if (userId is not null)
        {
            user = await _userService.FindByIdAsync(Guid.Parse(userId));
            var test = await _plantService.GetMostPopularPlantByLikes(userId);
            var test2 = await _plantService.GetMostPopularPlantByBookmarks(userId);
            var test3 = await _plantService.GetMostInteractedPlantByComments(userId);
            var test4 = await _plantService.GetMostPopularPlantsGlobally(3, user);
        }

        Enum.TryParse(requestDto.Type, out PlantType type);

        var plants = await _plantService.GetPlantsQuery(requestDto.Page, requestDto.Size, type,
            requestDto.SearchTerm, user);

        return Results.Ok(plants);
    }

    [HttpGet("most-popular")]
    public async Task<IResult> GetMostPopularPlants()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        User? user = null;
        
        if (userId is not null) {
            user = await _userService.FindByIdAsync(Guid.Parse(userId));
        }
        
        var plants = await _plantService.GetMostPopularPlantsGlobally(3, user);

        return Results.Ok(plants);
    }

    [HttpGet("bookmarks")]
    [Authorize(AuthorizationPolicies.Authenticated)]
    public async Task<IResult> GetBookmarkedPlants([FromQuery] PlantsRequestDto requestDto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId is null)
        {
            return Results.Unauthorized();
        }

        var user = await _userFeaturesService.GetUser(userId);

        if (user is null)
        {
            return Results.NotFound();
        }

        Enum.TryParse(requestDto.Type, out PlantType type);

        var plants = _userFeaturesService.GetBookmarkedPlants(user, requestDto.Page, requestDto.Size, type, requestDto.SearchTerm);

        return Results.Ok(plants);
    }

    [HttpGet("{plantId:guid}")]
    public async Task<IResult> GetPlantById(Guid plantId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var plant = await _plantService.GetPlantById(plantId);
        if (plant is null) return Results.NotFound($"Plant with ID: {plantId} not found.");
        var mappedPlant = _mapper.Map<PlantDto>(plant);

        if (userId is null)
        {
            return Results.Ok(mappedPlant);
        }

        var user = await _userService.FindByIdAsync(Guid.Parse(userId));
        var commentDtos = plant.Comments.Select(p => new PlantCommentDto
        {
            Id = p.Id,
            PlantId = p.PlantId,
            Content = p.Content,
            User = _mapper.Map<CommentUserInfoDto>(p.User),
            IsLiked = p.Likes.Contains(user),
            LikeCount = p.Likes.Count,
            LastModified = p.LastModified,
            CreatedAt = p.CreatedAt
        }).ToList();

        var plantDto = _mapper.Map<PlantDto>(plant);

        plantDto.Comments = commentDtos;
        plantDto.LikeCount = plant.Likes.Count;
        plantDto.IsLiked = plant.Likes.Contains(user);
        plantDto.IsBookmarked = plant.Bookmarks.Contains(user);

        return Results.Ok(plantDto);
    }

    [HttpPost("like-plant")]
    [Authorize(AuthorizationPolicies.Authenticated)]
    public async Task<IResult> LikePlant([FromBody] Guid plantId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var user = await _userService.FindByIdAsync(Guid.Parse(userId!));

        var plant = await _plantService.GetPlantById(plantId);
        if (plant is null) return Results.NotFound();

        var res = await _plantService.LikePlant(plant, user);

        return res ? Results.Ok() : Results.BadRequest();
    }

    [HttpPost("bookmark")]
    [Authorize(AuthorizationPolicies.Authenticated)]
    public async Task<IResult> BookmarkPlant([FromBody] Guid plantId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var user = await _userService.FindByIdAsync(Guid.Parse(userId!));

        var plant = await _plantService.GetPlantById(plantId);
        if (plant is null) return Results.NotFound();

        var res = await _plantService.BookmarkPlant(plant, user);

        return res ? Results.Ok() : Results.BadRequest();
    }

    // Endpoints relating to Plant Comments

    [HttpPost("comment")]
    [Authorize(AuthorizationPolicies.Authenticated)]
    public async Task<IResult> AddNewComment([FromBody] NewPlantCommentDto newPlantCommentDto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var user = await _userService.FindByIdAsync(Guid.Parse(userId!));

        var res = await _plantService.AddNewComment(user, newPlantCommentDto.PlantId, newPlantCommentDto.Content);

        return res ? Results.Ok() : Results.BadRequest();
    }

    [HttpPost("like-comment")]
    [Authorize(AuthorizationPolicies.Authenticated)]
    public async Task<IResult> LikePlantComment([FromBody] Guid plantCommentId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var user = await _userService.FindByIdAsync(Guid.Parse(userId!));

        var plantComment = await _plantService.GetPlantCommentById(plantCommentId);
        if (plantComment is null) return Results.NotFound();

        var res = await _plantService.LikePlantComment(plantComment, user);

        return res ? Results.Ok() : Results.BadRequest();
    }
}
