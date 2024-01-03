using Matt.Auditing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Acme.Persistence.Configs;

public abstract class CreationAuditedConfiguration<TEntity>
    : IEntityTypeConfiguration<TEntity>
    where TEntity : class, ICreationAuditedObject
{
    protected void ConfigureCreationAudited(EntityTypeBuilder<TEntity> builder)
    {
        builder.Property(r => r.CreationTime).IsRequired();
        builder.Property(r => r.CreatorId);
    }

    public abstract void Configure(EntityTypeBuilder<TEntity> builder);
}