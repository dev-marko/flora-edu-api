using System.Security.Claims;
using AutoMapper;
using FloraEdu.Application.Authentication.Interfaces;
using FloraEdu.Application.Services.Interfaces;
using FloraEdu.Domain.Authorization;
using FloraEdu.Domain.DataTransferObjects;
using FloraEdu.Domain.DataTransferObjects.Article;
using FloraEdu.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
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
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var article = await _blogService.GetArticleById(articleId);
        if (article is null) return Results.NotFound($"Article with ID: {articleId} not found.");
        var mappedArticle = _mapper.Map<ArticleDto>(article);

        if (userId is null)
        {
            return Results.Ok(mappedArticle);
        }

        var user = await _userService.FindByIdAsync(Guid.Parse(userId));
        var commentDtos = article.Comments.Select(c => new ArticleCommentDto
        {
            Id = c.Id,
            ArticleId = c.ArticleId,
            Content = c.Content,
            User = _mapper.Map<CommentUserInfoDto>(c.User),
            IsLiked = c.Likes.Contains(user),
            LikeCount = c.Likes.Count,
            LastModified = c.LastModified,
            CreatedAt = c.CreatedAt
        }).ToList();

        var articleDto = _mapper.Map<ArticleDto>(article);

        articleDto.Comments = commentDtos;
        articleDto.LikeCount = article.Likes.Count;
        articleDto.IsLiked = article.Likes.Contains(user);
        articleDto.IsBookmarked = article.Bookmarks.Contains(user);

        return Results.Ok(articleDto);
    }

    [HttpPost("like-article")]
    [Authorize(AuthorizationPolicies.Authenticated)]
    public async Task<IResult> LikeArticle([FromBody] Guid articleId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var user = await _userService.FindByIdAsync(Guid.Parse(userId!));

        var article = await _blogService.GetArticleById(articleId);
        if (article is null) return Results.NotFound();

        var res = await _blogService.LikeArticle(article, user);

        return res ? Results.Ok() : Results.BadRequest();
    }

    [HttpPost("bookmark")]
    [Authorize(AuthorizationPolicies.Authenticated)]
    public async Task<IResult> BookmarkArticle([FromBody] Guid articleId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var user = await _userService.FindByIdAsync(Guid.Parse(userId!));

        var article = await _blogService.GetArticleById(articleId);
        if (article is null) return Results.NotFound();

        var res = await _blogService.BookmarkArticle(article, user);

        return res ? Results.Ok() : Results.BadRequest();
    }

    // Endpoints relating to Article Comments

    [HttpPost("comment")]
    [Authorize(AuthorizationPolicies.Authenticated)]
    public async Task<IResult> AddNewComment([FromBody] NewArticleCommentDto newArticleCommentDto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var user = await _userService.FindByIdAsync(Guid.Parse(userId!));

        var res = await _blogService.AddNewComment(user, newArticleCommentDto.ArticleId, newArticleCommentDto.Content);

        return res ? Results.Ok() : Results.BadRequest();
    }

    [HttpPost("like-comment")]
    [Authorize(AuthorizationPolicies.Authenticated)]
    public async Task<IResult> LikeArticleComment([FromBody] Guid articleCommentId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var user = await _userService.FindByIdAsync(Guid.Parse(userId!));

        var articleComment = await _blogService.GetArticleCommentById(articleCommentId);
        if (articleComment is null) return Results.NotFound();

        var res = await _blogService.LikeArticleComment(articleComment, user);

        return res ? Results.Ok() : Results.BadRequest();
    }
}
