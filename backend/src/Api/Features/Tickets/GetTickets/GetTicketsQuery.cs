using MediatR;
using Shared.Contracts;
using Shared.Contracts.Sorting;
using Shared.Domain.Tickets;

namespace Api.Features.Tickets.GetTickets;

public sealed record GetTicketsQuery(
    int Page = 1,
    int PageSize = 20,
    string? Search = null,
    TicketFilter? Filter = null,
    TicketView View = TicketView.All,
    string? SortBy = null,
    SortDirection SortDirection = SortDirection.Desc)
    : IRequest<PagedResult<GetTicketsResponse>>;