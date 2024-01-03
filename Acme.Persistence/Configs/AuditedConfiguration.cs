using Matt.Auditing;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Acme.Persistence.Configs;

public abstract class AuditedConfiguration<TEntity>
    : CreationAuditedConfiguration<TEntity>
    where TEntity : class, IAuditedObject
{
    protected void ConfigureAudited(EntityTypeBuilder<TEntity> builder)
    {
        ConfigureCreationAudited(builder);
        builder.Property(r => r.CreationTime).IsRequired();
        builder.Property(r => r.CreatorId);
        builder.Property(r => r.LastModificationTime);
        builder.Property(r => r.LastModifierId);
    }
}