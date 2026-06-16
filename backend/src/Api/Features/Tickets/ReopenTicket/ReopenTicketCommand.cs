using MediatR;

namespace Api.Features.Tickets.ReopenTicket;

public record ReopenTicketCommand(
    Guid TicketId,
    Guid CompanyId) : IRequest;
