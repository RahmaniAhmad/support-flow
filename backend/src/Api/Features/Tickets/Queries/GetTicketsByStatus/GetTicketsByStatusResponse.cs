using Shared.Domain.Tickets;

namespace Api.Features.Tickets.Queries.GetTicketsByStatus;

public sealed record GetTicketsByStatusResponse(
    Guid Id,
    string Subject,
    string Description,
    Guid? CategoryId,
    string? CategoryName,
    TicketStatus Status,
    Guid? AssignedToUserId,
    DateTime CreatedAtUtc);