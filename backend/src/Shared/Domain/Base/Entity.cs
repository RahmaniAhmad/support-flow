namespace Shared.Domain.Base
{
    public abstract class Entity
    {
        private readonly Queue<IDomainEvent> pendingEvents = new();

        public Queue<IDomainEvent> DomainEvents => this.pendingEvents;

        public virtual Guid Id { get; protected set; } = Guid.NewGuid();

        public static bool operator ==(Entity? a, Entity? b)
        {
            if (a is null && b is null)
            {
                return true;
            }

            if (a is null || b is null)
            {
                return false;
            }

            return a.Equals(b);
        }

        public static bool operator !=(Entity a, Entity b)
        {
            return !(a == b);
        }

        public override bool Equals(object? obj)
        {
            if (obj is not Entity other)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (this.Id == Guid.Empty || other.Id == Guid.Empty)
            {
                return false;
            }

            return this.Id == other.Id;
        }

        public override int GetHashCode() => this.Id.GetHashCode();

        protected void AddDomainEvent(IDomainEvent domainEvent)
        {
            this.pendingEvents.Enqueue(domainEvent);
        }
    }
}
