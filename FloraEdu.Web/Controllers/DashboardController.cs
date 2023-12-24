﻿using System.Security.Claims;
using AutoMapper;
using Azure.Storage.Blobs.Models;
using FloraEdu.Application.Authentication.Interfaces;
using FloraEdu.Application.Services.Interfaces;
using FloraEdu.Domain.Authorization;
using FloraEdu.Domain.DataTransferObjects.Article;
using FloraEdu.Domain.DataTransferObjects.Plant;
using FloraEdu.Domain.Entities;
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
    private readonly IBlobStorageService _blobStorageService;
    private readonly IMapper _mapper;

    public DashboardController(IPlantService plantService, IUserService userService, IMapper mapper,
        IBlogService blogService, IBlobStorageService blobStorageService)
    {
        _blogService = blogService;
        _blobStorageService = blobStorageService;
        _plantService = plantService;
        _userService = userService;
        _mapper = mapper;
    }

    [HttpGet("plants")]
    public async Task<IResult> GetPlants([FromQuery] PlantsRequestDto requestDto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var user = await _userService.FindByIdAsync(Guid.Parse(userId!));
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

        var mostPopularByLikes = await _plantService.GetMostPopularPlantByLikes(userId);
        var mostPopularByBookmarks = await _plantService.GetMostPopularPlantByBookmarks(userId);
        var mostPopularByNumberOfComments = await _plantService.GetMostInteractedPlantByComments(userId);
        var mostPopularByUniqueVisitors = await _plantService.GetMostPopularPlantByUniqueVisitors(userId);
        var plantLikesChartData = await _plantService.GetPlantLikesChartData(userId);
        var plantBookmarksChartData = await _plantService.GetPlantBookmarksChartData(userId);

        var plantAnalytics = new PlantAnalytics
        {
            MostPopularByLikes = mostPopularByLikes.Item1,
            MostPopularByLikesCount = mostPopularByLikes.Item2,
            MostPopularByBookmarks = mostPopularByBookmarks.Item1,
            MostPopularByBookmarksCount = mostPopularByBookmarks.Item2,
            MostPopularByNumberOfComments = mostPopularByNumberOfComments.Item1,
            MostPopularByNumberOfCommentsCount = mostPopularByNumberOfComments.Item2,
            MostPopularByUniqueVisitors = mostPopularByUniqueVisitors.Item1,
            MostPopularByUniqueVisitorsCount = mostPopularByUniqueVisitors.Item2,
            LikesChartData = plantLikesChartData,
            BookmarksChartData = plantBookmarksChartData
        };

        return Results.Ok(plantAnalytics);
    }

    [HttpGet("article-analytics")]
    public async Task<IResult> GetArticleAnalyticsByCreator()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId is null) return Results.Unauthorized();

        var mostPopularByLikes = await _blogService.GetMostPopularArticleByLikes(userId);
        var mostPopularByBookmarks = await _blogService.GetMostPopularArticleByBookmarks(userId);
        var mostPopularByNumberOfComments = await _blogService.GetMostInteractedArticleByComments(userId);
        var mostPopularByUniqueVisitors = await _blogService.GetMostPopularArticleByUniqueVisitors(userId);
        var articleLikesChartData = await _blogService.GetArticleLikesChartData(userId);
        var articleBookmarksChartData = await _blogService.GetArticleBookmarksChartData(userId);

        var articleAnalytics = new ArticleAnalytics
        {
            MostPopularByLikes = mostPopularByLikes.Item1,
            MostPopularByLikesCount = mostPopularByLikes.Item2,
            MostPopularByBookmarks = mostPopularByBookmarks.Item1,
            MostPopularByBookmarksCount = mostPopularByBookmarks.Item2,
            MostPopularByNumberOfComments = mostPopularByNumberOfComments.Item1,
            MostPopularByNumberOfCommentsCount = mostPopularByNumberOfComments.Item2,
            MostPopularByUniqueVisitors = mostPopularByUniqueVisitors.Item1,
            MostPopularByUniqueVisitorsCount = mostPopularByUniqueVisitors.Item2,
            LikesChartData = articleLikesChartData,
            BookmarksChartData = articleBookmarksChartData
        };

        return Results.Ok(articleAnalytics);
    }

    [HttpGet("plant-thumbnail-sas-token")]
    public IResult GetPlantThumbnailImagesContainerUploadUri([FromQuery] string blobName)
    {
        var sasToken = _blobStorageService.GetPlantThumbnailUploadUri(blobName, AccessTier.Hot);
        return Results.Ok(sasToken);
    }

    [HttpGet("plant-header-sas-token")]
    public IResult GetPlantHeaderImagesContainerUploadUri([FromQuery] string blobName)
    {
        var sasToken = _blobStorageService.GetPlantHeaderImageUploadUri(blobName, AccessTier.Hot);
        return Results.Ok(sasToken);
    }

    [HttpPut("plants")]
    public async Task<IResult> UpdatePlant([FromBody] PlantEditDto plantDto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var user = await _userService.FindByIdAsync(Guid.Parse(userId!));

        var didParse = Enum.TryParse<PlantType>(plantDto.Type, out var plantType);
        Plant res;

        if (plantDto.IsNew)
        {
            var newPlant = new Plant
            {
                Name = plantDto.Name,
                Description = plantDto.Description,
                Type = didParse ? plantType : PlantType.Unknown,
                PlantImage = new PlantImage
                {
                    ThumbnailImageUrl = plantDto.ThumbnailImageUrl,
                    HeaderImageUrls = new List<string>
                    {
                        plantDto.HeaderImageUrl
                    }
                },
                Author = user,
                Predispositions = plantDto.Predispositions,
                Maintenance = plantDto.Maintenance,
                Planting = plantDto.Planting,
            };

            res = await _plantService.Create(newPlant);
        }
        else
        {
            var plantToEdit = await _plantService.GetById(plantDto.Id);

            plantToEdit.Name = plantDto.Name;
            plantToEdit.Description = plantDto.Description;
            plantToEdit.Type = didParse ? plantType : PlantType.Unknown;
            plantToEdit.PlantImage = new PlantImage
            {
                ThumbnailImageUrl = plantDto.ThumbnailImageUrl,
                HeaderImageUrls = new List<string>
                {
                    plantDto.HeaderImageUrl
                }
            };
            plantToEdit.Predispositions = plantDto.Predispositions;
            plantToEdit.Maintenance = plantDto.Maintenance;
            plantToEdit.Planting = plantDto.Planting;
            plantToEdit.LastModified = DateTime.UtcNow;

            res = await _plantService.Update(plantToEdit);
        }

        var mapped = _mapper.Map<PlantCardDto>(res);

        return Results.Ok(mapped);
    }

    [HttpDelete("plants")]
    public async Task<IResult> DeletePlant([FromBody] string plantId)
    {
        var deleted = await _plantService.Delete(Guid.Parse(plantId));

        return deleted ? Results.Ok() : Results.BadRequest();
    }
}
