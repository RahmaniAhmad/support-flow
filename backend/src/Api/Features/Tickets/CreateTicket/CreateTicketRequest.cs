namespace Api.Features.Tickets.CreateTicket;

public sealed record CreateTicketRequest(
    string Subject,
    string Description);
