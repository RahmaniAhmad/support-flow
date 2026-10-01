using FluentValidation;

namespace Api.Features.Tickets.Commands.ReopenTicket;

public sealed class ReopenTicketCommandValidator
    : AbstractValidator<ReopenTicketCommand>
{
    public ReopenTicketCommandValidator()
    {
        RuleFor(x => x.TicketId)
            .NotEmpty();
    }
}