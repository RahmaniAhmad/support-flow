using FluentValidation;

namespace Api.Features.Tickets.Commands.CloseTicket;

public sealed class CloseTicketCommandValidator
    : AbstractValidator<CloseTicketCommand>
{
    public CloseTicketCommandValidator()
    {
        RuleFor(x => x.TicketId)
            .NotEmpty();
    }
}