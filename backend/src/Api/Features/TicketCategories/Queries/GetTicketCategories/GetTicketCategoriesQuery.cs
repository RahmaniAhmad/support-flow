using MediatR;
using Shared.Contracts;

namespace Api.Features.TicketCategories.Queries.GetTicketCategories;

public sealed record GetTicketCategoriesQuery(
    int Page = 1,
    int PageSize = 20,
    string? Search = null)
    : IRequest<PagedResult<TicketCategoryResponse>>;
