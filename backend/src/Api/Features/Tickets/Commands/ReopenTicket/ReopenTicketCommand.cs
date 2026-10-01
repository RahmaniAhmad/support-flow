using MediatR;

namespace Api.Features.Tickets.Commands.ReopenTicket;

public record ReopenTicketCommand(
    Guid TicketId) : IRequest;
