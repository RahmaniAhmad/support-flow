using Shared.Domain.Base;

namespace Shared.Domain.Tickets.Events;

public sealed record TicketCategoryChangedDomainEvent(
    Guid TicketId,
    Guid CompanyId) : IDomainEvent;
