using FluentValidation;

namespace Api.Features.Tickets.ResolveTicket;

public sealed class ResolveTicketCommandValidator
    : AbstractValidator<ResolveTicketCommand>
{
    public ResolveTicketCommandValidator()
    {
        RuleFor(x => x.TicketId)
            .NotEmpty();
    }
}