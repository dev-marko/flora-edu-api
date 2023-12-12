using System.Net.Http.Json;
using System.Security.Claims;
using FloraEdu.Application.Authentication.Dtos;
using FloraEdu.Application.Authentication.Dtos.Extensions;
using FloraEdu.Application.Authentication.Interfaces;
using FloraEdu.Domain.Entities;
using FloraEdu.Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FloraEdu.Application.Authentication.Implementations;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<User> _signInManager;

    public UserService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager,
        SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
    }

    public async Task<IEnumerable<UserDto>> GetAll()
    {
        var users = await _userManager.Users.ToListAsync();
        var userDtos = users.Select(user => user.MapToDto());
        return userDtos;
    }

    public async Task<User> FindByNameAsync(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);
        if (user is null) throw new ApiException("User not found", ErrorCodes.UserNonExistant);
        return user;
    }

    public async Task<User> FindByIdAsync(Guid id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user is null) throw new ApiException("User not found", ErrorCodes.UserNonExistant);
        return user;
    }


    public async Task<User> FindByEmailAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user is null) throw new ApiException("User not found", ErrorCodes.UserNonExistant);
        return user;
    }

    public async Task Update(string id, UserDto user)
    {
        var userToUpdate = await _userManager.FindByIdAsync(id);
        if (userToUpdate is null) throw new ApiException("User not found", ErrorCodes.UserNonExistant);

        userToUpdate.FirstName = user.FirstName;
        userToUpdate.LastName = user.LastName;
        userToUpdate.Email = user.Email;
        userToUpdate.IsDeleted = user.IsDeleted;

        await _userManager.UpdateAsync(userToUpdate);
    }

    public async Task Delete(Guid id)
    {
        var userToDelete = await _userManager.FindByIdAsync(id.ToString());
        if (userToDelete is null) throw new ApiException("User not found", ErrorCodes.UserNonExistant);

        userToDelete.IsDeleted = true;
        await _userManager.UpdateAsync(userToDelete);
    }


    public async Task<bool> CheckPasswordAsync(string userName, string password)
    {
        var user = await _userManager.FindByNameAsync(userName);
        if (user is null) throw new ApiException("User not found", ErrorCodes.UserNonExistant);

        var passwordMatches = await _userManager.CheckPasswordAsync(user, password);
        if (!passwordMatches) throw new ApiException("Invalid password", ErrorCodes.PasswordMismatch);

        return passwordMatches;
    }

    public async Task<IList<string>> GetRolesAsync(LoginDto user)
    {
        var existingUser = await _userManager.FindByNameAsync(user.UserName);
        if (existingUser is null) throw new ApiException("User not found", ErrorCodes.UserNonExistant);
        return await _userManager.GetRolesAsync(existingUser);
    }

    public async Task<IdentityResult> CreateUserAsync(User user, string password)
    {
        var userExists = user.UserName != null && await UserExists(user.UserName);

        if (userExists) throw new ApiException("User already exists", ErrorCodes.UserExists);

        string? nameToUseForAvatarInitials;

        if (!string.IsNullOrEmpty(user.FirstName) && !string.IsNullOrEmpty(user.LastName))
        {
            nameToUseForAvatarInitials = string.Join('+', user.FirstName, user.LastName);
        }
        else
        {
            nameToUseForAvatarInitials = user.UserName;
        }

        var userAvatarImageUrl
            = $"https://ui-avatars.com/api?background=random&rounded=true&name={nameToUseForAvatarInitials}";

        user.AvatarImageUrl = userAvatarImageUrl;

        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded) throw new ApiException("CreateUser operation failed", ErrorCodes.OperationFailed);

        return result;
    }

    public async Task<bool> RoleExistsAsync(string roleName)
    {
        return await _roleManager.RoleExistsAsync(roleName);
    }

    public async Task<IdentityResult?> AddToRoleAsync(User user, string role)
    {
        if (!await _roleManager.RoleExistsAsync(role)) return null;
        await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, role));
        return await _userManager.AddToRoleAsync(user, role);
    }

    public async Task<IdentityResult> CreateRoleAsync(IdentityRole role)
    {
        return await _roleManager.CreateAsync(role);
    }

    public async Task<bool> UserExists(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);
        return user != null;
    }

    public async Task Login(string userName, string password, string jwt)
    {
        var user = await _userManager.FindByNameAsync(userName);
        if (user is null) throw new ApiException("User not found", ErrorCodes.UserNonExistant);
        await _signInManager.PasswordSignInAsync(userName, password, true, true);
        await _userManager.AddClaimAsync(user, new Claim("Name", userName));
    }

    public async Task Logout(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user is null) throw new ApiException("User not found", ErrorCodes.UserNonExistant);
        await _signInManager.SignOutAsync();
    }

    public async Task<ExternalLoginInfo?> GetExternalLoginInfoAsync()
    {
        return await _signInManager.GetExternalLoginInfoAsync();
    }

    public async Task<IdentityResult> AddLoginAsync(User user, UserLoginInfo login)
    {
        return await _userManager.AddLoginAsync(user, login);
    }

    public async Task SignInAsync(User user, bool isPersistent)
    {
        await _signInManager.SignInAsync(user, isPersistent);
    }
}
