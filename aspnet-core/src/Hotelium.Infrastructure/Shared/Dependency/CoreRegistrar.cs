using System.Reflection;

using Hotelium.Shared.Repositories.EntityFrameworkCore;
using Hotelium.Shared.Repositories.EntityFrameworkCore.Configuration;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hotelium.Shared.Dependency;

public class CoreRegistrar
{
    public static void Register(IServiceCollection services, IConfiguration configuration, IEnumerable<Assembly> assemblies)
    {
        EntityFrameworkCoreConfigurer.Configure(services, new EntityFrameworkCoreConfiguration(configuration));

        services.AddSingleton(typeof(ILogger<>), typeof(MicrosoftLogger<>));
        services.AddAutoMapper(assemblies);
    }
}