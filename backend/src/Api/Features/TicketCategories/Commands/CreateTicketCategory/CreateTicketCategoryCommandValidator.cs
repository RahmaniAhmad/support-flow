using FluentValidation;

namespace Api.Features.TicketCategories.Commands.CreateTicketCategory;

public sealed class CreateTicketCategoryCommandValidator
    : AbstractValidator<CreateTicketCategoryCommand>
{
    public CreateTicketCategoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Description)
            .MaximumLength(500);
    }
}
