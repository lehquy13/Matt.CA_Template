using Acme.Infrastructure.Persistence.EntityFrameworkCore;
using Matt.SharedKernel.Application.Contracts.Interfaces.Infrastructures;
using Matt.SharedKernel.Auditing;
using Matt.SharedKernel.Domain;
using Matt.SharedKernel.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Acme.Infrastructure.Persistence;

internal sealed class UnitOfWork(
    ILogger<UnitOfWork> logger,
    AppDbContext appDbContext,
    AuthDbContext authDbContext,
    ICurrentUserService currentUserService
) : IUnitOfWork
{
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Updating auditable entities...");
        UpdateAuditableEntities();

        logger.LogInformation("Saving changes...");

        var appSaved = await appDbContext.SaveChangesAsync(cancellationToken);
        var identitySaved = await authDbContext.SaveChangesAsync(cancellationToken);

        return appSaved + identitySaved;
    }

    private void UpdateAuditableEntities()
    {
        UpdateCreationTimeEntities();
        UpdateModificationTimeEntities();
    }

    private void UpdateCreationTimeEntities()
    {
        var hasCreationTimeEntries = appDbContext.ChangeTracker.Entries<IHasCreationTime>();

        foreach (var entityEntry in hasCreationTimeEntries)
        {
            if (entityEntry.State != EntityState.Added) continue;

            entityEntry.Property(e => e.CreationTime).CurrentValue = DateTimeProvider.Now;

            // If the entity is type of ICreationAuditedObject<T>, we should set CreatorId
            if (entityEntry.Entity is ICreationAuditedObject)
                entityEntry.Property(nameof(ICreationAuditedObject.CreatorId)).CurrentValue =
                    currentUserService.UserId.ToString();
        }
    }

    private void UpdateModificationTimeEntities()
    {
        var hasModificationTimeEntries = appDbContext.ChangeTracker.Entries<IHasModificationTime>();

        foreach (var entityEntry in hasModificationTimeEntries)
        {
            if (entityEntry.State is not (EntityState.Added or EntityState.Modified)) continue;

            entityEntry.Property(e => e.LastModificationTime).CurrentValue = DateTime.Now;

            if (entityEntry.Entity is IModificationAuditedObject)
                entityEntry.Property(nameof(IModificationAuditedObject.LastModifierId)).CurrentValue =
                    currentUserService.UserId.ToString();
        }
    }
}