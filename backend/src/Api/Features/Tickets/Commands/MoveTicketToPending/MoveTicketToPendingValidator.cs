using Api.Features.Tickets.Commands.MoveTicketToPending;
using FluentValidation;

namespace Api.Features.Tickets.Commands.CloseTicket;

public sealed class MoveTicketToPendingValidator
    : AbstractValidator<MoveTicketToPendingCommand>
{
    public MoveTicketToPendingValidator()
    {
        RuleFor(x => x.TicketId)
            .NotEmpty();
    }
}