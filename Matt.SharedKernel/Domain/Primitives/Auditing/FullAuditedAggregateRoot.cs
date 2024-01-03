using Matt.Auditing;

namespace Matt.SharedKernel.Domain.Primitives.Auditing;

public abstract class FullAuditedAggregateRoot<TId> : AuditedAggregateRoot<TId>,
    IFullAuditedObject
    where TId : notnull
{
    public bool IsDeleted { get; set; }

    public DateTime? DeletionTime { get; set; }

    public string? DeleterId { get; set; }
    
    protected FullAuditedAggregateRoot()
    {
    }
}