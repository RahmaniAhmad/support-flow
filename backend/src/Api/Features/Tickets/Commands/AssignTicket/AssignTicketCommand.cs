using MediatR;

namespace Api.Features.Tickets.Commands.AssignTicket;

public record AssignTicketCommand(
    Guid TicketId,
    Guid AssignedToUserId) : IRequest;

