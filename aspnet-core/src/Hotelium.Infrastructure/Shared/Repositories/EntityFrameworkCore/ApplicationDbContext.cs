using Microsoft.EntityFrameworkCore;

namespace Hotelium.Shared.Repositories.EntityFrameworkCore;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        modelBuilder.ApplyFilterByIsDeleted();
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);
        configurationBuilder.Properties<DateTime>().HaveColumnType("date");
        configurationBuilder.Properties<string>().HaveColumnType("varchar");
        configurationBuilder.Properties<decimal>().HavePrecision(18, 2);
    }
}