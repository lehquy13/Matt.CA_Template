using Matt.SharedKernel.Domain.Primitives.Abstractions;

namespace Matt.SharedKernel.Domain.Interfaces.Repositories;

public interface IRepository<TEntity, TId>
    : IReadOnlyRepository<TEntity, TId>
    where TEntity : class, IAggregateRoot<TId>
    where TId : notnull
{
    /// <summary>
    /// Find the entity by the entity's Id and return the entity with ChangeTracker
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<TEntity?> FindAsync(TId id, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Insert the entity to the database
    /// </summary>
    /// <param name="entity"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task InsertAsync(TEntity entity, CancellationToken cancellationToken = default(CancellationToken));

    /// <summary>
    /// Remove the entity from the database by the entity's Id
    /// </summary>
    /// <param name="spec"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> RemoveAsync(TId spec, CancellationToken cancellationToken = default(CancellationToken));

    Task RemoveManyAsync(IEnumerable<TId> ids, bool autoSave = false,
        CancellationToken cancellationToken = default(CancellationToken));
}