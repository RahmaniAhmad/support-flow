namespace Shared.Domain.Base
{
    public abstract class AggregateRoot : Entity
    {
        public virtual bool CanBeDeleted()
        {
            return false;
        }
    }
}
