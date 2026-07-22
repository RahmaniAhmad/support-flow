using MediatR;
using Shared.Caching;

namespace Api.Features.Tickets.ResolveTicket;

public record ResolveTicketCommand(
    Guid TicketId) : IRequest;

