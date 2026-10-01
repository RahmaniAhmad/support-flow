using FluentValidation;

namespace Api.Features.Tickets.Commands.CreateTicket;

public sealed class CreateTicketCommandValidator
    : AbstractValidator<CreateTicketCommand>
{
    public CreateTicketCommandValidator()
    {
        RuleFor(x => x.Subject)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MaximumLength(4000);
    }
}