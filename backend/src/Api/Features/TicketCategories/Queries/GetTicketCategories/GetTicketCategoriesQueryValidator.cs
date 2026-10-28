using FluentValidation;

namespace Api.Features.TicketCategories.Queries.GetTicketCategories;

public sealed class GetTicketCategoriesQueryValidator
    : AbstractValidator<GetTicketCategoriesQuery>
{
    public GetTicketCategoriesQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0);

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100);

        RuleFor(x => x.Search)
            .MaximumLength(100);
    }
}
