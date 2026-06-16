using Shared.Domain.Tickets;

namespace Api.Features.Tickets.GetTicket;

public sealed record GetTicketResponse(
    Guid Id,
    string Subject,
    string Description,
    TicketStatus Status,
    Guid? AssignedToUserId,
    DateTime CreatedAtUtc);