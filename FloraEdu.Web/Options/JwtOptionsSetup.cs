using FloraEdu.Application.Options;
using Microsoft.Extensions.Options;

namespace FloraEdu.Web.Options;

public class JwtOptionsSetup : IConfigureOptions<JwtOptions>
{
    private const string JwtSection = "Jwt";
    private readonly IConfiguration _configuration;

    public JwtOptionsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(JwtOptions options)
    {
        _configuration.GetSection(JwtSection).Bind(options);
    }
}
