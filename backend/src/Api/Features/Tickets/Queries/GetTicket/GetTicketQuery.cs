using MediatR;

namespace Api.Features.Tickets.Queries.GetTicket;

public sealed record GetTicketQuery(Guid TicketId)
    : IRequest<GetTicketResponse?>;