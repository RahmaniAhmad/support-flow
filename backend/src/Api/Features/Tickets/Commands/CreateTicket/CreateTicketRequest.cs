namespace Api.Features.Tickets.Commands.CreateTicket;

public sealed record CreateTicketRequest(
    string Subject,
    string Description);
