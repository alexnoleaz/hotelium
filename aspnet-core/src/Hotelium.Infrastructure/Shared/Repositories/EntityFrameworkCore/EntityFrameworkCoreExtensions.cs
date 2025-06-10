using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Hotelium.Shared.Entities;

namespace Hotelium.Shared.Repositories.EntityFrameworkCore;

public static class EntityFrameworkCoreExtensions
{
    public static ModelBuilder ApplyFilterByIsDeleted(this ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var clrType = entityType.ClrType;
            if (!typeof(ISoftDelete).IsAssignableFrom(clrType))
                continue;

            var parameter = Expression.Parameter(clrType, "e");
            var isDeletedProperty = Expression.Property(parameter, nameof(ISoftDelete.IsDeleted));
            var notDeleted = Expression.Not(isDeletedProperty);
            var lambda = Expression.Lambda(notDeleted, parameter);

            var entityMethod = typeof(ModelBuilder).GetMethod(nameof(ModelBuilder.Entity), Array.Empty<Type>());
            var genericEntityMethod = entityMethod!.MakeGenericMethod(clrType);
            var entityBuilder = genericEntityMethod.Invoke(modelBuilder, null)!;

            var hasQueryFilterMethod = entityBuilder.GetType().GetMethod("HasQueryFilter", new[] { lambda.GetType() });
            if (hasQueryFilterMethod is null)
                throw new InvalidOperationException($"Cannot find HasQueryFilter for {clrType.Name}");

            hasQueryFilterMethod.Invoke(entityBuilder, new object[] { lambda });
        }

        return modelBuilder;
    }
}