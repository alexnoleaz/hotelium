using Microsoft.Extensions.Configuration;

namespace Hotelium.Shared.Repositories.EntityFrameworkCore.Configuration;

public class EntityFrameworkCoreConfiguration(IConfiguration configuration)
    : IEntityFrameworkCoreConfiguration
{
    public string GetConnectionString() =>
        configuration.GetRequiredSection("ConnectionStrings:DefaultConnection").Value!;
}