using MediatR;

namespace Api.Features.Tickets.AssignTicket;

public record AssignTicketCommand(
    Guid TicketId,
    Guid CompanyId,
    Guid AssignedToUserId) : IRequest;
