using Shared.Domain.Tickets;

namespace Api.Features.Tickets.Queries.GetTickets;

public sealed record GetTicketsResponse(
    Guid Id,
    long TicketNumber,
    string Subject,
    string Description,
    Guid? CategoryId,
    string? CategoryName,
    TicketStatus Status,
    Guid? AssignedToUserId,
    string? AssigneeName,
    string CompanyName,
    string CreatedBy,
    DateTime CreatedAtUtc);