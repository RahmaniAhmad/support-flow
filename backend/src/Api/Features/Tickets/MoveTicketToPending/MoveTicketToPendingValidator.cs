using Api.Features.Tickets.MoveTicketToPending;
using FluentValidation;

namespace Api.Features.Tickets.CloseTicket;

public sealed class MoveTicketToPendingValidator
    : AbstractValidator<MoveTicketToPendingCommand>
{
    public MoveTicketToPendingValidator()
    {
        RuleFor(x => x.TicketId)
            .NotEmpty();
    }
}