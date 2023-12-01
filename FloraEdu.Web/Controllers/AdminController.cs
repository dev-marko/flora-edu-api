using FloraEdu.Application.Authentication.Dtos;
using FloraEdu.Application.Authentication.Interfaces;
using FloraEdu.Domain.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FloraEdu.Web.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(AuthorizationPolicies.Admin)]
public class AdminController : ControllerBase
{
    private readonly IUserService _userService;

    public AdminController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("role")]
    public async Task<IActionResult> CreateRole([FromBody] RoleDto role)
    {
        var newRole = new IdentityRole
        {
            Name = role.Name
        };
        
        var result = await _userService.CreateRoleAsync(newRole);

        return Ok(result);
    }
}
