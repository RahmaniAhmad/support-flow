using FluentValidation;

namespace Api.Features.Tickets.Commands.AssignTicket;

public sealed class AssignTicketCommandValidator
    : AbstractValidator<AssignTicketCommand>
{
    public AssignTicketCommandValidator()
    {
        RuleFor(x => x.TicketId)
            .NotEmpty();

        RuleFor(x => x.AssignedToUserId)
            .NotEmpty();
    }
}