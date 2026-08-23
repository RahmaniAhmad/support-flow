using FluentValidation;

namespace Api.Features.Users.CreateUser;

public sealed class CreateUserCommandValidator
    : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8);

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Phone)
            .MaximumLength(30)
            .When(x => !string.IsNullOrWhiteSpace(x.Phone));

        RuleFor(x => x.Role)
            .IsInEnum();
    }
}