using Shared.Contracts;
using Shared.Domain.Tickets;

namespace Api.Features.Tickets.GetTickets;

public sealed record GetTicketsRequest(
    int Page = 1,
    int PageSize = 20,
    string? Search = null,
    TicketFilter? Status = null,
    TicketView View = TicketView.All,
    string? SortBy = null,
    string? SortDirection = null);