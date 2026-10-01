using MediatR;
using Shared.Domain.Tickets;

namespace Api.Features.Tickets.Queries.GetTicketsByStatus;

public sealed record GetTicketsByStatusQuery(TicketStatus Status)
    : IRequest<List<GetTicketsByStatusResponse>>;