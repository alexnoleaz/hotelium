using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text.Json;
using Hotelium.Auth.JwtBearer;
using Hotelium.Shared;

namespace Hotelium.Auth;

public static class AuthConfigurer
{
    public static void Configure(IServiceCollection services, IConfiguration configuration)
    {
        if (!configuration.GetRequiredSection("App:JwtBearer:IsEnabled").Get<bool>())
            return;

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
        {
            var tokenConfiguration = new TokenAuthConfiguration(configuration);

            options.Audience = tokenConfiguration.Audience;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = tokenConfiguration.SecurityKey,

                ValidateIssuer = true,
                ValidIssuer = tokenConfiguration.Issuer,

                ValidateAudience = true,
                ValidAudience = tokenConfiguration.Audience,

                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,

                RoleClaimType = ClaimTypes.Role
            };

            options.Events = new JwtBearerEvents
            {
                OnChallenge = context =>
                {
                    context.HandleResponse();
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    context.Response.ContentType = "application/json";

                    var response = Response.Error("Token is missing or invalid.", HttpStatusCode.Unauthorized);
                    return context.Response.WriteAsync(JsonSerializer.Serialize(response));
                },
                OnForbidden = context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    context.Response.ContentType = "application/json";

                    var response = Response<string>.Fail("You don't have permission to access this resource.", HttpStatusCode.Forbidden);
                    return context.Response.WriteAsync(JsonSerializer.Serialize(response));
                }
            };
        });

    }
}