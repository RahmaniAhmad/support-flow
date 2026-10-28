using FluentValidation;

namespace Api.Features.TicketCategories.Commands.UpdateTicketCategory;

public sealed class UpdateTicketCategoryCommandValidator
    : AbstractValidator<UpdateTicketCategoryCommand>
{
    public UpdateTicketCategoryCommandValidator()
    {
        RuleFor(x => x.CategoryId)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Description)
            .MaximumLength(500);
    }
}
