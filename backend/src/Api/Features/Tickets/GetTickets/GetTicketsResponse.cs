using Shared.Domain.Tickets;

namespace Api.Features.Tickets.GetTickets;

public sealed record GetTicketsResponse(
    Guid Id,
    long TicketNumber,
    string Subject,
    string Description,
    TicketStatus Status,
    string? AssigneeName,
    string CompanyName,
    DateTime CreatedAtUtc);