using MediatR;

namespace Api.Features.Tickets.GetTickets;

public sealed record GetTicketsQuery()
    : IRequest<List<GetTicketsResponse>>;