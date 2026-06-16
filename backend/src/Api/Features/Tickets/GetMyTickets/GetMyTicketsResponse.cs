using Shared.Domain.Tickets;

namespace Api.Features.Tickets.GetMyTickets;

public sealed record GetMyTicketsResponse(
    Guid Id,
    string Subject,
    string Description,
    TicketStatus Status,
    Guid? AssignedToUserId,
    DateTime CreatedAtUtc);