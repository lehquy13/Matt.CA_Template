using Acme.Persistence.EntityFrameworkCore;
using Matt.AutoDI;
using Matt.SharedKernel.Domain.Interfaces;
using Matt.SharedKernel.Domain.Interfaces.Repositories;
using Matt.SharedKernel.Domain.Primitives;
using Matt.SharedKernel.Domain.Primitives.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Acme.Persistence.Repository;

internal class RepositoryImpl<TEntity, TId> :
    ReadOnlyRepositoryImpl<TEntity, TId>, IRepository<TEntity, TId>,
    IOpenGenericService<IRepository<TEntity, TId>>
    where TEntity : Entity<TId>, IAggregateRoot<TId>
    where TId : notnull
{
    public RepositoryImpl(AppDbContext appDbContext,
        IAppLogger<RepositoryImpl<TEntity, TId>> logger)
        : base(appDbContext, logger)
    {
    }

    public async Task InsertAsync(TEntity entity)
    {
        try
        {
            await AppDbContext.Set<TEntity>().AddAsync(entity);
        }
        catch (Exception ex)
        {
            Logger.LogError(ErrorMessage, "Error in InsertAsync", ex.Message);
        }
    }

    public async Task<bool> RemoveAsync(TId id)
    {
        try
        {
            var deleteRecord = await AppDbContext.Set<TEntity>().FirstOrDefaultAsync(x => x.Id.Equals(id));

            if (deleteRecord == null) return false;

            AppDbContext.Set<TEntity>().Remove(deleteRecord);
            return true;
        }
        catch (Exception ex)
        {
            Logger.LogError(ErrorMessage, "GetAllListAsync", ex.Message);
            return new bool();
        }
    }

    public async Task<TEntity?> FindAsync(TId id)
    {
        try
        {
            return await AppDbContext
                .Set<TEntity>()
                .FirstOrDefaultAsync(x => x.Id.Equals(id));
        }
        catch (Exception ex)
        {
            Logger.LogError(ErrorMessage, "Error in FindAsync", ex.Message);
            return null;
        }
    }
}