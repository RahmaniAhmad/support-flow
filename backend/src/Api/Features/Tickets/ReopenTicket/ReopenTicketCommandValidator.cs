using FluentValidation;

namespace Api.Features.Tickets.ReopenTicket;

public sealed class ReopenTicketCommandValidator
    : AbstractValidator<ReopenTicketCommand>
{
    public ReopenTicketCommandValidator()
    {
        RuleFor(x => x.TicketId)
            .NotEmpty();
    }
}