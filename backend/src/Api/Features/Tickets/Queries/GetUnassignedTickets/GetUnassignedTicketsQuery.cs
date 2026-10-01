using MediatR;

namespace Api.Features.Tickets.Queries.GetUnassignedTickets;

public sealed record GetUnassignedTicketsQuery()
    : IRequest<List<GetUnassignedTicketsResponse>>;