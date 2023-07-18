using System.ComponentModel.DataAnnotations;

namespace FloraEdu.Application.Authentication.Dtos;

public class LoginDto
{
    [Required(ErrorMessage = "Username is required")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }

    public LoginDto(string userName, string password)
    {
        UserName = userName;
        Password = password;
    }
}
