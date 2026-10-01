using Shared.Domain.Tickets;

namespace Api.Features.Tickets.Queries.GetTicketsByStatus;

public sealed record GetTicketsByStatusResponse(
    Guid Id,
    string Subject,
    string Description,
    TicketStatus Status,
    Guid? AssignedToUserId,
    DateTime CreatedAtUtc);