using MediatR;
using Shared.Caching;

namespace Api.Features.Tickets.AssignTicket;

public record AssignTicketCommand(
    Guid TicketId,
    Guid CompanyId,
    Guid AssignedToUserId) : IRequest, IInvalidateCacheCommand
{
    public string[] CacheGroupKeys => new[]
    {
        $"tickets:company:{CompanyId}",
        $"tickets:company:{CompanyId}:ticket:{TicketId}",
        $"tickets:company:{CompanyId}:user:{AssignedToUserId}",
        $"dashboard:{CompanyId}"
    };
}
