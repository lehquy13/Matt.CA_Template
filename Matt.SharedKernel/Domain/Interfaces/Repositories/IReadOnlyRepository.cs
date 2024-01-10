using Matt.AutoDI;
using Matt.SharedKernel.Domain.Primitives.Abstractions;
using Matt.SharedKernel.Domain.Specifications.Interfaces;

namespace Matt.SharedKernel.Domain.Interfaces.Repositories;

//TODO: Consider to change IEntity to IAggregateRoot
public interface IReadOnlyRepository<TEntity, TId> : IScoped
    where TEntity : class, IEntity<TId> where TId : notnull
{
    Task<TEntity?> GetAsync(ISpecification<TEntity> spec, CancellationToken cancellationToken = default);

    Task<List<TEntity>> GetListAsync(CancellationToken cancellationToken = default);

    Task<List<TEntity>> GetListAsync(IPaginatedGetListSpecification<TEntity> spec,
        CancellationToken cancellationToken = default);

    Task<long> CountAsync(CancellationToken cancellationToken = default);

    Task<long> CountAsync(ISpecification<TEntity> spec, CancellationToken cancellationToken = default);
}