using MediatR;

namespace Api.Features.Tickets.GetUnassignedTickets;

public sealed record GetUnassignedTicketsQuery()
    : IRequest<List<GetUnassignedTicketsResponse>>;