namespace Api.Features.Tickets.Commands.AssignTicket;

public sealed record AssignTicketRequest(
    Guid AssignedToUserId);
