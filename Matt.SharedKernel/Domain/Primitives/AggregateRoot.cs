using Matt.SharedKernel.Domain.Primitives.Abstractions;

namespace Matt.SharedKernel.Domain.Primitives;

public abstract class AggregateRoot<TId> : Entity<TId>, IAggregateRoot<TId>
    where TId : notnull
{
    protected AggregateRoot(TId id)
    {
        Id = id;
    }

#pragma warning disable CS8618

    protected AggregateRoot()
    {
    }

#pragma warning restore CS8618
}