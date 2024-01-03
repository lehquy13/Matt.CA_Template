namespace Matt.SharedKernel.Domain.Primitives.Abstractions;

public interface IAggregateRoot<TId> : IEntity<TId> where TId : notnull
{
}