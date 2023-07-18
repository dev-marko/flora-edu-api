using FloraEdu.Application.Authentication.Dtos;
using FloraEdu.Application.Authentication.Interfaces;
using FloraEdu.Domain.Entities;
using FloraEdu.Domain.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FloraEdu.Web.Controllers;

[Route("api/authentication")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IJwtProvider _jwtProvider;
    private readonly IUserService _userService;

    public AuthenticationController(UserManager<User> userManager, SignInManager<User> signInManager,
        IJwtProvider jwtProvider, IUserService userService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtProvider = jwtProvider;
        _userService = userService;
    }

    [HttpGet]
    public IActionResult Hello()
    {
        return Ok("Hello World!");
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto model)
    {
        if (await _userService.UserExists(model.Email))
            // throw new ApiException(ApiErrorType.UserAlreadyExistsException, nameof(AuthenticateController));
            throw new InvalidOperationException("User already exists");
        User user = new()
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            UserName = model.UserName,
            Email = model.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
        };

        await _userService.CreateUserAsync(user, model.Password);

        await _userService.AddToRoleAsync(user, UserRoles.User);

        return Ok(user);
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto model)
    {
        var user = await _userService.FindByNameAsync(model.UserName);
        await _userService.CheckPasswordAsync(model.UserName, model.Password);
        var token = _jwtProvider.GenerateJwt(user);

        var response = new
        {
            AccessToken = token,
            UserId = user.Id
        };
        return Ok(response);
    }
}
