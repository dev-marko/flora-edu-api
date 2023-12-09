using System.Security.Claims;
using AutoMapper;
using FloraEdu.Application.Authentication.Interfaces;
using FloraEdu.Application.Services.Interfaces;
using FloraEdu.Domain.DataTransferObjects.Article;
using FloraEdu.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FloraEdu.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class ArticlesController : ControllerBase
{
    private readonly IBlogService _blogService;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public ArticlesController(IBlogService blogService, IUserService userService, IMapper mapper)
    {
        _blogService = blogService;
        _userService = userService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IResult> Get([FromQuery] ArticlesRequestDto articlesRequestDto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        User? user = null;

        if (userId is not null)
        {
            user = await _userService.FindByIdAsync(Guid.Parse(userId));
        }

        var articles = await _blogService.GetArticlesQuery(articlesRequestDto.Page, articlesRequestDto.Size,
            articlesRequestDto.SearchTerm, user);

        return Results.Ok(articles);
    }

    [HttpGet("{articleId:guid}")]
    public async Task<IResult> GetArticleById(Guid articleId)
    {
        var article = await _blogService.GetArticleById(articleId);
        if (article is null) return Results.NotFound($"Article with ID: {articleId} not found.");
        var mappedArticle = _mapper.Map<ArticleDto>(article);

        return Results.Ok(mappedArticle);
    }
}
