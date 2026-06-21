using MediatR;

namespace Api.Features.Tickets.GetTickets;

public sealed record GetTicketsQuery(Guid CompanyId)
    : IRequest<List<GetTicketsResponse>>;