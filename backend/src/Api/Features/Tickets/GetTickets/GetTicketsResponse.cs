using Shared.Domain.Tickets;

namespace Api.Features.Tickets.GetTickets;

public sealed record GetTicketsResponse(
    Guid Id,
    long TicketNumber,
    string Subject,
    string Description,
    TicketStatus Status,
    Guid? AssignedToUserId,
    string? AssigneeName,
    string CompanyName,
    string CreatedBy,
    DateTime CreatedAtUtc);