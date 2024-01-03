using Matt.SharedKernel.Domain.Primitives.Abstractions;

namespace Matt.SharedKernel.Domain.Primitives;

public abstract class Entity<TId> : IEntity<TId>
    where TId : notnull
{
    public TId Id { get; protected set; }

    protected Entity(TId id)
    {
        Id = id;
    }

#pragma warning disable CS8618

    protected Entity()
    {
    }

#pragma warning restore CS8618
}