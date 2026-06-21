using MediatR;

namespace Api.Features.Tickets.GetUnassignedTickets;

public sealed record GetUnassignedTicketsQuery(Guid CompanyId)
    : IRequest<List<GetUnassignedTicketsResponse>>;