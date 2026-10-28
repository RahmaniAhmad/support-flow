namespace Api.Features.TicketCategories.Queries.GetTicketCategories;

public sealed record GetTicketCategoriesRequest(
    int Page = 1,
    int PageSize = 20,
    string? Search = null);
