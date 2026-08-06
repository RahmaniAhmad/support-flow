using MediatR;

namespace Api.Features.Tickets.MoveTicketToPending;

public sealed record MoveTicketToPendingCommand(
    Guid TicketId) : IRequest;