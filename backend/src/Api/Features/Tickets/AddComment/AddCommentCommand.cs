using MediatR;

namespace Api.Features.Tickets.AddComment;

public record AddCommentCommand(
    Guid TicketId,
    string Content) : IRequest<Guid>;
