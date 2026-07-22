using MediatR;

namespace Api.Features.Tickets.GetMyTickets;

public sealed record GetMyTicketsQuery()
    : IRequest<List<GetMyTicketsResponse>>;