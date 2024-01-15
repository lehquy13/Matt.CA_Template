using Acme.Persistence.EntityFrameworkCore;
using Matt.SharedKernel.Application.Contracts.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Acme.Persistence.Repository;

internal class AsyncQueryableExecutor(
    AppDbContext dbContext
)
    : IAsyncQueryableExecutor
{
    public async Task<List<T>> ToListAsync<T>(IQueryable<T> queryable, CancellationToken cancellationToken = default)
    {
        return await queryable.ToListAsync(cancellationToken);
    }
}