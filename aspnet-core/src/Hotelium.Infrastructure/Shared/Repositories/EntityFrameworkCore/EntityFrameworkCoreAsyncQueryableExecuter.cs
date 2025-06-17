using Microsoft.EntityFrameworkCore;
using Hotelium.Shared.Dependency;
using Hotelium.Shared.Linq;

namespace Hotelium.Shared.Repositories.EntityFrameworkCore;

public class EntityFrameworkCoreAsyncQueryableExecuter : IAsyncQueryableExecuter, ISingletonDependency
{
    public Task<int> CountAsync<T>(IQueryable<T> queryable)
        => ExecuteAsync(queryable, (q, token) => q.CountAsync(token));

    public Task<List<T>> ToListAsync<T>(IQueryable<T> queryable)
        => ExecuteAsync(queryable, (q, token) => q.ToListAsync(token));

    public Task<T?> FirstOrDefaultAsync<T>(IQueryable<T> queryable)
        => ExecuteAsync(queryable, (q, token) => q.FirstOrDefaultAsync(token));

    public Task<bool> AnyAsync<T>(IQueryable<T> queryable)
        => ExecuteAsync(queryable, (q, token) => q.AnyAsync(token));

    private async Task<TResult> ExecuteAsync<T, TResult>(
        IQueryable<T> queryable,
        Func<IQueryable<T>, CancellationToken, Task<TResult>> executeMethod
    )
        => await executeMethod(queryable, default).ConfigureAwait(false);
}