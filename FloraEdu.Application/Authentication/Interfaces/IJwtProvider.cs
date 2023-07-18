using FloraEdu.Domain.Entities;

namespace FloraEdu.Application.Authentication.Interfaces;

public interface IJwtProvider
{
    string GenerateJwt(User user);
}
