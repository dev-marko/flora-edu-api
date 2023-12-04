using System.Security.Claims;
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

    public ArticlesController(IBlogService blogService, IUserService userService)
    {
        _blogService = blogService;
        _userService = userService;
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
}
