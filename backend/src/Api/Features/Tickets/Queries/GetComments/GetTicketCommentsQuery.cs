using MediatR;

namespace Api.Features.Tickets.Queries.GetComments;

public sealed record GetTicketCommentsQuery(Guid TicketId)
    : IRequest<List<GetTicketCommentsResponse>?>;