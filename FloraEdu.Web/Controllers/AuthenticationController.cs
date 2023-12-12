using AutoMapper;
using FloraEdu.Application.Authentication.Dtos;
using FloraEdu.Application.Authentication.Interfaces;
using FloraEdu.Domain.Authorization;
using FloraEdu.Domain.DataTransferObjects;
using FloraEdu.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FloraEdu.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IJwtProvider _jwtProvider;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public AuthenticationController(IJwtProvider jwtProvider, IUserService userService, IMapper mapper)
    {
        _jwtProvider = jwtProvider;
        _userService = userService;
        _mapper = mapper;
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto model)
    {
        User user = new()
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            UserName = model.UserName,
            Email = model.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
        };

        await _userService.CreateUserAsync(user, model.Password);

        await _userService.AddToRoleAsync(user, Roles.RegularUser);

        return Ok(user);
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto model)
    {
        var user = await _userService.FindByNameAsync(model.UserName);
        await _userService.CheckPasswordAsync(model.UserName, model.Password);
        var token = await _jwtProvider.GenerateJwt(user);

        var roles = await _userService.GetRolesAsync(model);

        var response = new LoginResponse
        {
            AccessToken = token,
            UserInfo = new UserInfo
            {
                UserName = user.UserName!,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email!,
                AvatarUrl = user.AvatarImageUrl,
                Roles = roles.ToList()
            }
        };

        return Ok(response);
    }
}
