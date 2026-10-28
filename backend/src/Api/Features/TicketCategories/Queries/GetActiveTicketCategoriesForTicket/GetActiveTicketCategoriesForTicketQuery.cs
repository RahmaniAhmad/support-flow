using MediatR;

namespace Api.Features.TicketCategories.Queries.GetActiveTicketCategoriesForTicket;

public sealed record GetActiveTicketCategoriesForTicketQuery(Guid TicketId)
    : IRequest<IReadOnlyList<TicketCategoryResponse>>;
