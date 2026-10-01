using MediatR;

namespace Api.Features.Tickets.Commands.CloseTicket;

public record CloseTicketCommand(
    Guid TicketId) : IRequest;
