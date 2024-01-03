using Matt.Auditing;

namespace Matt.SharedKernel.Domain.Primitives.Auditing;

public abstract class AuditedAggregateRoot<TId> : AggregateRoot<TId>, IAuditedObject
    where TId : notnull
{
    //Creation
    public DateTime CreationTime { get; init; }

    public string? CreatorId { get; set; }

    //Modification
    public DateTime? LastModificationTime { get; }

    public string? LastModifierId { get; set; }

    protected AuditedAggregateRoot(TId id) : base(id)
    {
        CreationTime = DateTime.Now.ToLocalTime();
        LastModificationTime = DateTime.Now.ToLocalTime();
    }

    protected AuditedAggregateRoot()
    {
    }
}