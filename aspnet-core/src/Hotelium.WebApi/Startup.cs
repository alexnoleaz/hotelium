using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;
using Hotelium.Shared.Dependency;
using Hotelium.Shared;

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
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseCors(_defaultCorsPolicyName);
        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(cfg => cfg.MapControllers());
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
        });
    }
}