using MediatR;

namespace Api.Features.TicketCategories.Queries.GetTicketCategoryById;

public sealed record GetTicketCategoryByIdQuery(Guid CategoryId)
    : IRequest<TicketCategoryResponse>;
