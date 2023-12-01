using FloraEdu.Domain.Entities;

namespace FloraEdu.Application.Authentication.Interfaces;

public interface IJwtProvider
{
    Task<string> GenerateJwt(User user);
}
