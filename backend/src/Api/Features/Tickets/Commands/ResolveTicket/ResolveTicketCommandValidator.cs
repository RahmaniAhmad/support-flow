using FluentValidation;

namespace Api.Features.Tickets.Commands.ResolveTicket;

public sealed class ResolveTicketCommandValidator
    : AbstractValidator<ResolveTicketCommand>
{
    public ResolveTicketCommandValidator()
    {
        RuleFor(x => x.TicketId)
            .NotEmpty();
    }
}