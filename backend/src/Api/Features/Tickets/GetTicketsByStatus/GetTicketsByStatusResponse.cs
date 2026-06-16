using Shared.Domain.Tickets;

namespace Api.Features.Tickets.GetTicketsByStatus;

public sealed record GetTicketsByStatusResponse(
    Guid Id,
    string Subject,
    string Description,
    TicketStatus Status,
    Guid? AssignedToUserId,
    DateTime CreatedAtUtc);