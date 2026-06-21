using MediatR;
using Shared.Caching;

namespace Api.Features.Tickets.AddComment;

public record AddCommentCommand(
    Guid TicketId,
    Guid UserId,
    Guid CompanyId,
    string Content) : IRequest<Guid>, IInvalidateCacheCommand
{
    public string[] CacheGroupKeys => new[]
    {
        $"tickets:company:{CompanyId}",
        $"tickets:comments:{TicketId}"
    };

}
