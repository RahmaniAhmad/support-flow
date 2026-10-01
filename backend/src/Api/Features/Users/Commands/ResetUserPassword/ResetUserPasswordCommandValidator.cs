using FluentValidation;

namespace Api.Features.Users.Commands.ResetUserPassword;

public sealed class ResetUserPasswordCommandValidator
    : AbstractValidator<ResetUserPasswordCommand>
{
    public ResetUserPasswordCommandValidator()
    {
        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8);
    }
}