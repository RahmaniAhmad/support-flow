using Shared.Domain.Base;

namespace Shared.Domain.Tickets.Events;

public sealed record TicketCategoryUpdatedDomainEvent(
    Guid CategoryId,
    Guid CompanyId) : IDomainEvent;
