using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Hotelium.Shared.Entities;

namespace Hotelium.Shared.Repositories.EntityFrameworkCore;

public static class EntityFrameworkCoreExtensions
{
    public static ModelBuilder ApplyFilterByIsDeleted(this ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
            {
                var parameter = Expression.Parameter(entityType.ClrType, "e");
                var body = Expression.Not(Expression.Property(parameter, "IsDeleted"));
                var lambda = Expression.Lambda(body, parameter);

                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
            }

        return modelBuilder;
    }
}