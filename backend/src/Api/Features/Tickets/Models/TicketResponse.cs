namespace Api.Features.Tickets.Models;

public sealed record TicketResponse(
    Guid Id,
    string Subject,
    string Description,
    string Status,
    Guid? AssignedToUserId,
    DateTime CreatedAtUtc);