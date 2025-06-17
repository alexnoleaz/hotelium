using System.Linq.Expressions;
using System.Reflection;

using Hotelium.Shared.Services.Dto;

namespace Hotelium.Shared.Linq;

public static class QueryableExtensions
{
    public static IQueryable<T> PageBy<T>(this IQueryable<T> query, int skipCount, int maxResultCount)
    {
        if (query is null)
            throw new ArgumentNullException("query");

        return query.Skip(skipCount).Take(maxResultCount);
    }

    public static IQueryable<T> PageBy<T>(this IQueryable<T> query, IPagedResultRequest pagedResultRequest)
        => query.PageBy(pagedResultRequest.SkipCount, pagedResultRequest.MaxResultCount);

    public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, Expression<Func<T, bool>> predicate)
        => condition ? query.Where(predicate) : query;

    public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, Expression<Func<T, int, bool>> predicate)
        => condition ? query.Where(predicate) : query;

    public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName)
        => ApplyOrder(source, propertyName, "OrderBy");

    public static IQueryable<T> OrderByDescending<T>(this IQueryable<T> source, string propertyName)
        => ApplyOrder(source, propertyName, "OrderByDescending");

    private static IQueryable<T> ApplyOrder<T>(IQueryable<T> source, string propertyName, string methodName)
    {
        var type = typeof(T);
        var parameter = Expression.Parameter(type, "x");
        var property = type.GetProperty(propertyName,
                        BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

        if (property is null)
            throw new ArgumentException($"Property '{propertyName}' not found on type '{type.Name}'");

        var propertyAccess = Expression.MakeMemberAccess(parameter, property);
        var orderByExpression = Expression.Lambda(propertyAccess, parameter);
        var result = typeof(Queryable).GetMethods()
            .Where(m => m.Name == methodName && m.GetParameters().Length == 2)
            .Single()
            .MakeGenericMethod(type, property.PropertyType)
            .Invoke(null, new object[] { source, orderByExpression });

        return (IQueryable<T>)result!;
    }
}