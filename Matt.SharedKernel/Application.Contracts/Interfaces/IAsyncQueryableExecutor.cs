using Matt.AutoDI;

namespace Matt.SharedKernel.Application.Contracts.Interfaces;

public interface IAsyncQueryableExecutor : IScoped
{
    Task<List<T>> ToListAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default);
}