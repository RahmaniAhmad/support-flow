using Api.Authorization;
using Api.Filters;
using MediatR;
using Shared.Domain.Users;

namespace Api.Features.Tickets.Commands.ChangeTicketPriority;

public static class ChangeTicketPriorityEndpoint
{
    public static IEndpointRouteBuilder MapChangeTicketPriority(
        this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPut(
                "/tickets/{ticketId:guid}/priority",
                async (
                    Guid ticketId,
                    ChangeTicketPriorityRequest request,
                    ISender sender,
                    CancellationToken cancellationToken) =>
                {
                    await sender.Send(
                        new ChangeTicketPriorityCommand(
                            ticketId,
                            request.Priority),
                        cancellationToken);

                    return Results.NoContent();
                })
            .RequireAuthorization()
            .AddEndpointFilter<SecurityFilter>()
            .RequirePermission(Permissions.TicketsManagePriority)
            .WithTags("Tickets")
            .WithSummary("Change ticket priority");

        return endpoints;
    }
}
