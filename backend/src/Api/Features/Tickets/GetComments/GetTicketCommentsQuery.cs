using MediatR;

namespace Api.Features.Tickets.GetComments;

public sealed record GetTicketCommentsQuery(Guid TicketId)
    : IRequest<List<GetTicketCommentsResponse>?>;