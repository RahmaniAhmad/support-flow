using FluentValidation;

namespace Api.Features.TicketCategories.Queries.GetActiveTicketCategoriesForTicket;

public sealed class GetActiveTicketCategoriesForTicketQueryValidator
    : AbstractValidator<GetActiveTicketCategoriesForTicketQuery>
{
    public GetActiveTicketCategoriesForTicketQueryValidator()
    {
        RuleFor(x => x.TicketId)
            .NotEmpty();
    }
}
