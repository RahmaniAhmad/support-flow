using MediatR;

namespace Api.Features.Tickets.GetMyTickets;

public sealed record GetMyTicketsQuery(Guid CompanyId, Guid UserId)
    : IRequest<List<GetMyTicketsResponse>>;