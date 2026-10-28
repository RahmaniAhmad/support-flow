using Shared.Domain.Tickets;

namespace Api.Features.Tickets.Queries.GetTicket;

public sealed record GetTicketResponse(
     Guid Id,
     long TicketNumber,
     string Subject,
     string Description,
     Guid? CategoryId,
     string? CategoryName,
     TicketStatus Status,
     Guid? AssignedToUserId,
     string? AssigneeName,
     string CreatedByName,
     DateTime CreatedAtUtc,
     DateTime? UpdatedAtUtc);