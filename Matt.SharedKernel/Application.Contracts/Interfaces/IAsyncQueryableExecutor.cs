using Matt.AutoDI;

namespace Matt.SharedKernel.Application.Contracts.Interfaces;

public interface IAsyncQueryableExecutor : IScoped
{
    Task<List<T>> ToListAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default);
    Task<T?> FirstOrDefaultAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default);
    Task<long> LongCountAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default);
}