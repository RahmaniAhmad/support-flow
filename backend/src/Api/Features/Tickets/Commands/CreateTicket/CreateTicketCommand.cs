using MediatR;

namespace Api.Features.Tickets.Commands.CreateTicket;

public record CreateTicketCommand(
    string Subject,
    string Description) : IRequest<Guid>;
