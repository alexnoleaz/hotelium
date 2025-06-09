namespace Hotelium.Shared.Repositories.EntityFrameworkCore.Configuration;

public interface IEntityFrameworkCoreConfiguration
{
    string GetConnectionString();
}