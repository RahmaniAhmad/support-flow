using Shared.Domain.Tickets;

namespace Api.Features.Tickets.GetTickets;

public sealed record GetTicketsResponse(
    Guid Id,
    string Subject,
    string Description,
    TicketStatus Status,
    Guid? AssignedToUserId,
    DateTime CreatedAtUtc);