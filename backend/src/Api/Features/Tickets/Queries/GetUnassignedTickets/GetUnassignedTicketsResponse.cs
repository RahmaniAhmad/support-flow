using Shared.Domain.Tickets;

namespace Api.Features.Tickets.Queries.GetUnassignedTickets;

public sealed record GetUnassignedTicketsResponse(
    Guid Id,
    string Subject,
    string Description,
    Guid? CategoryId,
    string? CategoryName,
    TicketStatus Status,
    Guid? AssignedToUserId,
    DateTime CreatedAtUtc);