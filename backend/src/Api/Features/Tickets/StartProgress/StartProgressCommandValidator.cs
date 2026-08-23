using FluentValidation;

namespace Api.Features.Tickets.StartProgress;

public sealed class StartProgressCommandValidator
    : AbstractValidator<StartProgressCommand>
{
    public StartProgressCommandValidator()
    {
        RuleFor(x => x.TicketId)
            .NotEmpty();
    }
}