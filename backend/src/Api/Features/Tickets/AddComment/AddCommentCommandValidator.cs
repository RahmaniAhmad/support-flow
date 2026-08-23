using FluentValidation;

namespace Api.Features.Tickets.AddComment;

public sealed class AddCommentCommandValidator
    : AbstractValidator<AddCommentCommand>
{
    public AddCommentCommandValidator()
    {
        RuleFor(x => x.TicketId)
            .NotEmpty();

        RuleFor(x => x.Content)
            .NotEmpty()
            .MaximumLength(5000);
    }
}