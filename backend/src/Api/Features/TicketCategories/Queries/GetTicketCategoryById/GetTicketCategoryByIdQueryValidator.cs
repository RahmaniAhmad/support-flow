using FluentValidation;

namespace Api.Features.TicketCategories.Queries.GetTicketCategoryById;

public sealed class GetTicketCategoryByIdQueryValidator
    : AbstractValidator<GetTicketCategoryByIdQuery>
{
    public GetTicketCategoryByIdQueryValidator()
    {
        RuleFor(x => x.CategoryId)
            .NotEmpty();
    }
}
