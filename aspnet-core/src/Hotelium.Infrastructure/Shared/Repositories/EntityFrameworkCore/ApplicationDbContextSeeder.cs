using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Hotelium.Roles;
using Hotelium.Users;

namespace Hotelium.Shared.Repositories.EntityFrameworkCore;

public static class ApplicationDbContextSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

        await context.Database.MigrateAsync();
        await CreateRolesAsync(context);
        await CreateUsersAsync(context, passwordHasher);
    }

    private static async Task CreateUsersAsync(ApplicationDbContext context, IPasswordHasher passwordHasher)
    {
        if (await context.Users.AnyAsync())
            return;

        var adminRole = await context.Roles.FirstAsync(r => r.Name == "Admin");
        var modRole = await context.Roles.FirstAsync(r => r.Name == "Mod");
        var userRole = await context.Roles.FirstAsync(r => r.Name == "User");

        var users = new[]
        {
            new User
            {
                Name = "Admin",
                Surname = "Admin",
                Email = "admin@diars.com",
                Password = passwordHasher.HashPassword("admin123qwe"),
                Roles = new[] { adminRole }
            },
            new User
            {
                Name = "Mod",
                Surname = "Mod",
                Email = "mod@diars.com",
                Password = passwordHasher.HashPassword("mod123qwe"),
                Roles = new[] { modRole }
            },
            new User
            {
                Name = "User",
                Surname = "User",
                Email = "user@diars.com",
                Password = passwordHasher.HashPassword("user123qwe"),
                Roles = new[] { userRole }
            }
        };

        await context.Users.AddRangeAsync(users);
        await context.SaveChangesAsync();
    }

    private static async Task CreateRolesAsync(ApplicationDbContext context)
    {
        if (await context.Roles.AnyAsync())
            return;

        var roles = new[]
        {
            new Role { Name = "Admin" },
            new Role { Name = "Mod" },
            new Role { Name = "User" }
        };

        await context.Roles.AddRangeAsync(roles);
        await context.SaveChangesAsync();
    }
}