using MediatR;
using Shared.Contracts;
using Shared.Domain.Tickets;

namespace Api.Features.Tickets.GetTickets;

public sealed record GetTicketsQuery(Guid CompanyId,
    int Page = 1,
    int PageSize = 20,
    string? Search = null,
    TicketStatus? Status = null,
    Guid? AssignedToUserId = null,
    string? SortBy = null,
    bool Descending = true)
    : IRequest<PagedResult<GetTicketsResponse>>;