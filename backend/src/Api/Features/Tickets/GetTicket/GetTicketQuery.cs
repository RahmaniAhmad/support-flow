using MediatR;

namespace Api.Features.Tickets.GetTicket;

public sealed record GetTicketQuery(Guid TicketId)
    : IRequest<GetTicketResponse?>;