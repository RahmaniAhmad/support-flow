using MediatR;

namespace Api.Features.Tickets.ResolveTicket;

public record ResolveTicketCommand(
    Guid TicketId,
    Guid CompanyId) : IRequest;
