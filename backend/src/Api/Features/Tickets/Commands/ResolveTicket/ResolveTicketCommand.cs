using MediatR;

namespace Api.Features.Tickets.Commands.ResolveTicket;

public record ResolveTicketCommand(
    Guid TicketId) : IRequest;

