using MediatR;

namespace Api.Features.Tickets.CreateTicket;

public record CreateTicketCommand(
    Guid CompanyId,
    Guid UserId,
    string Subject,
    string Description) : IRequest<Guid>;
