using FluentValidation;

namespace Api.Features.Tickets.Commands.ChangeTicketPriority;

public sealed class ChangeTicketPriorityCommandValidator
    : AbstractValidator<ChangeTicketPriorityCommand>
{
    public ChangeTicketPriorityCommandValidator()
    {
        RuleFor(x => x.TicketId)
            .NotEmpty();

        RuleFor(x => x.Priority)
            .IsInEnum();
    }
}
