using FluentValidation;

namespace Api.Features.Tickets.Commands.StartProgress;

public sealed class StartProgressCommandValidator
    : AbstractValidator<StartProgressCommand>
{
    public StartProgressCommandValidator()
    {
        RuleFor(x => x.TicketId)
            .NotEmpty();
    }
}