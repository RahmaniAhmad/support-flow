using FluentValidation;

namespace Api.Features.TicketCategories.Commands.ActivateTicketCategory;

public sealed class ActivateTicketCategoryCommandValidator
    : AbstractValidator<ActivateTicketCategoryCommand>
{
    public ActivateTicketCategoryCommandValidator()
    {
        RuleFor(x => x.CategoryId)
            .NotEmpty();
    }
}
