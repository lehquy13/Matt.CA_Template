using Matt.SharedKernel.Domain.Interfaces;
using Matt.SharedKernel.Domain.Primitives.Abstractions;

namespace Matt.SharedKernel.Domain.Primitives;

public abstract class AggregateRoot<TId> : Entity<TId>, IAggregateRoot<TId>, IHasDomainEvents
    where TId : notnull
{
    private readonly List<IDomainEvent> _domainEvents = new();

    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    
    protected void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    protected AggregateRoot(TId id)
    {
        Id = id;
    }

    protected AggregateRoot()
    {
    }
}