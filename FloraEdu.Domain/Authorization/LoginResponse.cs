using FloraEdu.Domain.DataTransferObjects;

namespace FloraEdu.Domain.Authorization;

public class LoginResponse
{
    public required string AccessToken { get; set; }
    public required UserInfo UserInfo { get; set; }
}
