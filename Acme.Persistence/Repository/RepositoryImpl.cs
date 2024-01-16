﻿using Acme.Persistence.EntityFrameworkCore;
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

    public async Task InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        try
        {
            await AppDbContext.Set<TEntity>().AddAsync(entity, cancellationToken);
        }
        catch (Exception ex)
        {
            Logger.LogError(ErrorMessage, "Error in InsertAsync", ex.Message);
        }
    }

    public async Task<bool> RemoveAsync(TId id, CancellationToken cancellationToken = default)
    {
        try
        {
            var deleteRecord = await AppDbContext.Set<TEntity>().FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken: cancellationToken);

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

    public async Task RemoveManyAsync(IEnumerable<TId> ids, bool autoSave = false,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        try
        {
            var deleteRecords = AppDbContext.Set<TEntity>().Where(x => ids.Contains(x.Id));

            AppDbContext.Set<TEntity>().RemoveRange(deleteRecords);

            if (autoSave)
            {
                await AppDbContext.SaveChangesAsync(cancellationToken);
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ErrorMessage, "GetAllListAsync", ex.Message);
        }
    }

    public async Task<TEntity?> FindAsync(TId id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await AppDbContext
                .Set<TEntity>()
                .FirstOrDefaultAsync(x => x.Id.Equals(id), cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            Logger.LogError(ErrorMessage, "Error in FindAsync", ex.Message);
            return null;
        }
    }
}