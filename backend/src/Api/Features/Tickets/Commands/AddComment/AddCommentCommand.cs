using MediatR;

namespace Api.Features.Tickets.Commands.AddComment;

public record AddCommentCommand(
    Guid TicketId,
    string Content) : IRequest<Guid>;
