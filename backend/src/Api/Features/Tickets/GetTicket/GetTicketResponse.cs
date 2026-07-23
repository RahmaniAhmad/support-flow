using Shared.Domain.Tickets;

namespace Api.Features.Tickets.GetTicket;

public sealed record GetTicketResponse(
     Guid Id,
     string Subject,
     string Description,
     TicketStatus Status,
     Guid? AssignedToUserId,
     string? AssigneeName,
     string CreatedByName,
     DateTime CreatedAtUtc,
     DateTime? UpdatedAtUtc);