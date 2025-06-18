using Hotelium.Roles;
using Hotelium.Users;

using Microsoft.EntityFrameworkCore;

namespace Hotelium.Shared.Repositories.EntityFrameworkCore;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        modelBuilder.ApplyFilterByIsDeleted();
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        base.ConfigureConventions(configurationBuilder);
        configurationBuilder.Properties<string>().HaveColumnType("VARCHAR").HaveMaxLength(150);
        configurationBuilder.Properties<decimal>().HavePrecision(18, 2);
    }
}