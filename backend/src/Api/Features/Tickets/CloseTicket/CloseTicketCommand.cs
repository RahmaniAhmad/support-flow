using MediatR;

namespace Api.Features.Tickets.CloseTicket;

public record CloseTicketCommand(
    Guid TicketId) : IRequest;
