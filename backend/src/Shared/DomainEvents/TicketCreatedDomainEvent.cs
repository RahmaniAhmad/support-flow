
using Shared.Domain.Base;

namespace Shared.DomainEvents
{
    public sealed record TicketCreatedDomainEvent(
        Guid TicketId,
        Guid CompanyId,
        string Subject) : IDomainEvent;
}