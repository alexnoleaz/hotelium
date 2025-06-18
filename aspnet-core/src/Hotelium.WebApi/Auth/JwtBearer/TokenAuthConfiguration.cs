using System.Text;
using Microsoft.IdentityModel.Tokens;
using Hotelium.Shared.Dependency;

namespace Hotelium.Auth.JwtBearer;

public class TokenAuthConfiguration : ISingletonDependency
{
    public SymmetricSecurityKey SecurityKey { get; }
    public string Issuer { get; }
    public string Audience { get; }
    public SigningCredentials SigningCredentials { get; }
    public TimeSpan Expiration { get; }

    public TokenAuthConfiguration(IConfiguration configuration)
    {
        SecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration.GetRequiredSection("App:JwtBearer:SecurityKey").Get<string>()!));
        Issuer = configuration.GetRequiredSection("App:JwtBearer:Issuer").Get<string>()!;
        Audience = configuration.GetRequiredSection("App:JwtBearer:Audience").Get<string>()!;
        SigningCredentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);
        Expiration = TimeSpan.FromDays(1);
    }
}