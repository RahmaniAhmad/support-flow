using MediatR;

namespace Api.Features.Tickets.Commands.MoveTicketToPending;

public sealed record MoveTicketToPendingCommand(
    Guid TicketId) : IRequest;