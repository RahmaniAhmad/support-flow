using MediatR;

namespace Api.Features.Tickets.AddComment;

public record AddCommentCommand(
    Guid TicketId,
    Guid UserId,
    Guid CompanyId,
    string Content) : IRequest<Guid>;
