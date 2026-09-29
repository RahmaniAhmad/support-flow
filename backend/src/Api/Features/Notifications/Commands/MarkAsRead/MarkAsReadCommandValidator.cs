using FluentValidation;

namespace Api.Features.Notifications.Commands.MarkAsRead;

public sealed class MarkAsReadCommandValidator
    : AbstractValidator<MarkAsReadCommand>
{
    public MarkAsReadCommandValidator()
    {
        RuleFor(x => x.NotificationId)
            .NotEmpty();
    }
}