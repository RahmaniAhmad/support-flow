using FluentValidation;

namespace Api.Features.Users.ResetUserPassword;

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