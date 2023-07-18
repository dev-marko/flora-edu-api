using System.ComponentModel.DataAnnotations;

namespace FloraEdu.Application.Authentication.Dtos;

public class UserDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    [EmailAddress] public string Email { get; set; } = "";
    public bool IsDeleted { get; set; }
}
