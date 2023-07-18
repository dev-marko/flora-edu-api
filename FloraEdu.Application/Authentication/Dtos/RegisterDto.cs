using System.ComponentModel.DataAnnotations;

namespace FloraEdu.Application.Authentication.Dtos;

public class RegisterDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    [Required(ErrorMessage = "Username is required")]
    public string UserName { get; set; }

    [EmailAddress]
    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }

    public RegisterDto(string? firstName, string? lastName, string userName, string email, string password)
    {
        FirstName = firstName;
        LastName = lastName;
        UserName = userName;
        Email = email;
        Password = password;
    }
}
