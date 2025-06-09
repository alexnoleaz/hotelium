using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Hotelium.Shared.Repositories.EntityFrameworkCore.Configuration;

namespace Hotelium.Shared.Repositories.EntityFrameworkCore;

public static class EntityFrameworkCoreConfigurer
{
    public static void Configure(IServiceCollection services, IEntityFrameworkCoreConfiguration configuration)
    {
        services
            .AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString()))
            .AddScoped(typeof(IRepository<,>), typeof(EntityFrameworkCoreRepositoryBase<,>))
            .AddScoped(typeof(IRepository<>), typeof(EntityFrameworkCoreRepositoryBase<>));
    }
}