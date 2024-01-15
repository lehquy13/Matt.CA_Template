namespace Matt.SharedKernel.Domain.Interfaces;

public interface IHasDomainEvents
{
    public IReadOnlyList<IDomainEvent> DomainEvents { get; }

    public void ClearDomainEvents();
}