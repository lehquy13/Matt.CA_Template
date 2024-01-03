using Matt.SharedKernel.Domain.Primitives.Abstractions;

namespace Matt.SharedKernel.Domain.Interfaces.Repositories;

public interface IRepository<TEntity, TId> 
    : IReadOnlyRepository<TEntity, TId>
    where TEntity : class, IAggregateRoot<TId>
    where TId : notnull
{
    /// <summary>
    /// Get all the record of tables and able to query with linq due to the Queryable return
    /// Consider to remove this method
    /// </summary>
    IQueryable<TEntity> GetAll();

    /// <summary>
    /// Find the entity by the entity's Id and return the entity with ChangeTracker
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<TEntity?> FindAsync(TId id);

    /// <summary>
    /// Insert the entity to the database
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task InsertAsync(TEntity entity);

    /// <summary>
    /// Remove the entity from the database by the entity's Id
    /// </summary>
    /// <param name="spec"></param>
    /// <returns></returns>
    Task<bool> RemoveAsync(TId spec);
}