using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

using Hotelium.Shared.Dependency;

namespace Hotelium.Auth.JwtBearer;

public class JwtTokenService : ISingletonDependency
{
    private readonly TokenAuthConfiguration _configuration;

    public JwtTokenService(TokenAuthConfiguration configuration) => _configuration = configuration;

    public string Generate(long userId, string[]? roles = null)
        => CreateAccessToken(CreateJwtClaims(userId, roles));

    private string CreateAccessToken(IEnumerable<Claim> claims, TimeSpan? expiration = null)
    {
        var now = DateTime.UtcNow;
        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _configuration.Issuer,
            audience: _configuration.Audience,
            claims: claims,
            notBefore: now,
            expires: now.Add(expiration ?? _configuration.Expiration),
            signingCredentials: _configuration.SigningCredentials
        );

        return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
    }

    private List<Claim> CreateJwtClaims(long userId, string[]? roles = null)
    {
        Console.WriteLine(roles);
        var claims = new List<Claim>();
        claims.AddRange(new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        });

        if (roles is not null && roles.Length > 0)
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

        return claims;
    }
}