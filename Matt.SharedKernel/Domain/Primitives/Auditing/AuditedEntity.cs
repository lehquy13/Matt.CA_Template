using Matt.Auditing;

namespace Matt.SharedKernel.Domain.Primitives.Auditing;

public abstract class AuditedEntity<TId> : CreationAuditedEntity<TId>, IAuditedObject
    where TId : notnull
{
    //Modification
    public DateTime? LastModificationTime { get; }

    public string? LastModifierId { get; private set; }

    protected AuditedEntity(TId id) : base(id)
    {
        LastModificationTime = DateTime.Now.ToLocalTime();
    }

    protected AuditedEntity()
    {
    }
}