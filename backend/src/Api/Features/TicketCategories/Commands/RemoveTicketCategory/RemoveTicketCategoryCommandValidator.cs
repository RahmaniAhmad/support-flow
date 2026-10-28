using FluentValidation;

namespace Api.Features.TicketCategories.Commands.RemoveTicketCategory;

public sealed class RemoveTicketCategoryCommandValidator
    : AbstractValidator<RemoveTicketCategoryCommand>
{
    public RemoveTicketCategoryCommandValidator()
    {
        RuleFor(x => x.TicketId)
            .NotEmpty();
    }
}
