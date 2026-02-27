using System.Collections.Concurrent;

namespace DocumentComposition.Domain;

public abstract class AggregateRoot<TId> where TId : notnull
{
    private readonly ConcurrentQueue<object> domainEvents = new();

    public TId Id { get; protected set; } = default!;

    protected void Raise(object @event) => domainEvents.Enqueue(@event);

    public IReadOnlyList<object> DomainEvents => domainEvents.ToList().AsReadOnly();

    public void ClearEvents() => domainEvents.Clear();
}
