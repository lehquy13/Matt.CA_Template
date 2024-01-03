using Matt.Auditing;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Acme.Persistence.Configs;

public abstract class FullAuditedConfiguration<TEntity> : AuditedConfiguration<TEntity>
    where TEntity : class, IFullAuditedObject
{
    protected void ConfigureFullAudited(EntityTypeBuilder<TEntity> builder)
    {
        ConfigureAudited(builder);
        builder.Property(r => r.IsDeleted).IsRequired();
        builder.Property(r => r.DeletionTime);
        builder.Property(r => r.DeleterId);
    }
}