using Shared.Domain.Tickets;

namespace Api.Features.Tickets.GetUnassignedTickets;

public sealed record GetUnassignedTicketsResponse(
    Guid Id,
    string Subject,
    string Description,
    TicketStatus Status,
    Guid? AssignedToUserId,
    DateTime CreatedAtUtc);