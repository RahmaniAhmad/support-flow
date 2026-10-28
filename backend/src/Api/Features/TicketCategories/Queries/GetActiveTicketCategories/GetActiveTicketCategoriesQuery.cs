using MediatR;

namespace Api.Features.TicketCategories.Queries.GetActiveTicketCategories;

public sealed record GetActiveTicketCategoriesQuery
    : IRequest<IReadOnlyList<TicketCategoryResponse>>;
