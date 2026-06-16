
using Shared.Domain.Base;

namespace Shared.Domain.Tickets.Events;

public sealed record TicketCreatedDomainEvent(
    Guid TicketId,
    Guid CompanyId,
    string Subject) : IDomainEvent;
