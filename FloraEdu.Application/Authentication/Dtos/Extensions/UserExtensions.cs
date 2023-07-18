using FloraEdu.Domain.Entities;

namespace FloraEdu.Application.Authentication.Dtos.Extensions;

public static class UserExtensions
{
    public static UserDto MapToDto(this User user)
    {
        return new UserDto
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email ?? "",
            IsDeleted = user.IsDeleted
        };
    }
}
