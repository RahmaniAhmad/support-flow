using Shared.Domain.Tickets;

namespace Api.Features.Tickets.GetTickets;

public sealed class GetTicketsRequest
{
    public int Page { get; init; } = 1;

    public int PageSize { get; init; } = 20;

    public string? Search { get; init; }

    public TicketStatus? Status { get; init; }

    public Guid? AssignedToUserId { get; init; }

    public string? SortBy { get; init; }

    public bool? Descending { get; init; } = true;
}