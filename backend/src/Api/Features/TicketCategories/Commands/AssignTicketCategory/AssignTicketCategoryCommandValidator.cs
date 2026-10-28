using FluentValidation;

namespace Api.Features.TicketCategories.Commands.AssignTicketCategory;

public sealed class AssignTicketCategoryCommandValidator
    : AbstractValidator<AssignTicketCategoryCommand>
{
    public AssignTicketCategoryCommandValidator()
    {
        RuleFor(x => x.TicketId)
            .NotEmpty();

        RuleFor(x => x.CategoryId)
            .NotEmpty();
    }
}
