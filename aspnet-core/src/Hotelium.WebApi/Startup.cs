using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;
using Hotelium.Shared.Dependency;
using Hotelium.Shared;
using Hotelium.Shared.Repositories.EntityFrameworkCore;
using Hotelium.Shared.Filters;
using Hotelium.Auth;

public class Startup(IConfiguration appConfiguration, IWebHostEnvironment hostingEnvironment)
{
    private const string _defaultCorsPolicyName = "localhost";
    private const string _apiVersion = "v1";

    private readonly IConfiguration _appConfiguration = appConfiguration;
    private readonly IWebHostEnvironment _hostingEnvironment = hostingEnvironment;

    public void ConfigureServices(IServiceCollection services)
    {
        var assemblies = new[] {
            typeof(Startup).Assembly, // WebApi assembly
            typeof(ConventionalRegistrar).Assembly, // Infrastructure assembly
            typeof(ISingletonDependency).Assembly // Application assembly
        };

        services.AddControllers(cfg => cfg.Filters.Add<GlobalExceptionFilter>());

        AuthConfigurer.Configure(services, _appConfiguration);

        services.AddCors(
            options => options.AddPolicy(
                _defaultCorsPolicyName,
                builder => builder
                    .WithOrigins(
                        _appConfiguration.GetRequiredSection("App:CorsOrigins").Value!
                            .Split(",", StringSplitOptions.RemoveEmptyEntries)
                            .ToArray()
                    )
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
            )
        );

        services.AddConventionalServices(assemblies);
        services.AddCoreServices(_appConfiguration, assemblies);
        services.AddValidatorsFromAssemblies(assemblies);
        services.AddFluentValidationAutoValidation();

        ConfigureSwagger(services);

        services.Configure<ApiBehaviorOptions>(
            options => options.InvalidModelStateResponseFactory = context =>
            {
                var errors = context.ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .ToDictionary(kvp => kvp.Key, kvp => kvp.Value!.Errors.Select(e => e.ErrorMessage).ToArray());

                var response = Response<Dictionary<string, string[]>>.Fail(errors);
                return new BadRequestObjectResult(response);
            }
        );
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint($"/swagger/{_apiVersion}/swagger.json", $"Hotelium API {_apiVersion}");
                options.DisplayRequestDuration();
                options.EnablePersistAuthorization();
            });
        }

        app.UseHttpsRedirection();
        app.UseCors(_defaultCorsPolicyName);
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(cfg => cfg.MapControllers());

        ApplicationDbContextSeeder.SeedAsync(app.ApplicationServices).GetAwaiter().GetResult();
    }

    public void ConfigureSwagger(IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc(_apiVersion, new OpenApiInfo
            {
                Version = _apiVersion,
                Title = "Hotelium API",
                Description = "Hotelium API for the Hotelium application, built with ASP.NET Core.",
                Contact = new OpenApiContact
                {
                    Name = "Alexander Nole",
                    Email = string.Empty,
                    Url = new Uri("https://www.linkedin.com/in/alexnoleaz")
                }
            });
            options.DocInclusionPredicate((docName, description) => true);

            options.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "bearerAuth"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
    }
}