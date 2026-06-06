using Shared.Domain;

namespace Api.Features.Tickets.UpdateStatus
{
    public sealed record UpdateTicketStatusRequest(
        TicketStatus Status);
}