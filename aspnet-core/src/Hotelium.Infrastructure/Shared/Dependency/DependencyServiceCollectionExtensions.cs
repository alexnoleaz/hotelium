using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Hotelium.Shared.Dependency;

public static class DependencyServiceCollectionExtensions
{
    public static IServiceCollection AddConventionalServices(this IServiceCollection services, IEnumerable<Assembly> assemblies)
    {
        foreach (var assembly in assemblies)
            ConventionalRegistrar.RegisterAssemblyByConvention(services, assembly);

        return services;
    }

    public static IServiceCollection AddCoreServices(
            this IServiceCollection services,
            IConfiguration configuration,
            IEnumerable<Assembly> assemblies)
    {
        CoreRegistrar.Register(services, configuration, assemblies);
        return services;
    }
}