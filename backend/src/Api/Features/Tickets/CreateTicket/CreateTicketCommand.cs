using MediatR;
using Shared.Caching;

namespace Api.Features.Tickets.CreateTicket;

public record CreateTicketCommand(
    Guid CompanyId,
    Guid UserId,
    string Subject,
    string Description) : IRequest<Guid>, IInvalidateCacheCommand
{
    public string[] CacheGroupKeys => new[]
    {
        $"tickets:company:{CompanyId}",
        $"dashboard:{CompanyId}"
    };
}
