using FluentValidation;

namespace Api.Features.TicketCategories.Commands.DeactivateTicketCategory;

public sealed class DeactivateTicketCategoryCommandValidator
    : AbstractValidator<DeactivateTicketCategoryCommand>
{
    public DeactivateTicketCategoryCommandValidator()
    {
        RuleFor(x => x.CategoryId)
            .NotEmpty();
    }
}
