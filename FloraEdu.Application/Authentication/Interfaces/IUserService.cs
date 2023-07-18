using FloraEdu.Application.Authentication.Dtos;
using FloraEdu.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace FloraEdu.Application.Authentication.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAll();
    Task<User> FindByNameAsync(string userName);
    Task<User> FindByIdAsync(Guid id);
    Task<User> FindByEmailAsync(string email);
    Task Update(string id, UserDto entity);
    Task Delete(Guid id);
    Task<bool> CheckPasswordAsync(string userName, string password);
    Task<IList<string>> GetRolesAsync(LoginDto user);
    Task<IdentityResult> CreateUserAsync(User user, string password);
    Task<bool> RoleExistsAsync(string roleName);
    Task<IdentityResult?> AddToRoleAsync(User user, string role);
    Task<IdentityResult> CreateRoleAsync(IdentityRole role);
    Task<bool> UserExists(string userName);
    Task Login(string userName, string password, string jwt);
    Task Logout(Guid userId);
    Task<ExternalLoginInfo?> GetExternalLoginInfoAsync();
    Task<IdentityResult> AddLoginAsync(User user, UserLoginInfo login);
    Task SignInAsync(User user, bool isPersistent);
}
