using Shared.Domain.Tickets;

namespace Api.Features.Tickets.GetTickets;

public sealed record GetTicketsRequest(
    int Page = 1,
    int PageSize = 20,
    string? Search = null,
    TicketStatus? Status = null,
    TicketView View = TicketView.All,
    string? SortBy = null,
    bool? Descending = true);