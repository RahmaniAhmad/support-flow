using Shared.Abstractions;

namespace Shared.Events
{
    public sealed record TicketCreated(
        Guid TicketId,
        Guid CompanyId,
        string Subject) : IDomainEvent;
}