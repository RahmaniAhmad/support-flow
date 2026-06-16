namespace Shared.Domain.Base;

public abstract class AggregateRoot : Entity
{
    private readonly Queue<IDomainEvent> _domainEvents = [];

    public IReadOnlyCollection<IDomainEvent> DomainEvents =>
        _domainEvents.ToList();

    protected void AddDomainEvent(
        IDomainEvent domainEvent)
    {
        _domainEvents.Enqueue(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}

