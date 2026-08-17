using Api.Authorization;
using MediatR;
using Shared.Authentication;
using Shared.Domain.Users;

namespace Api.Features.Dashboard.Unassigned;

public static class UnassignedTicketsEndpoint
{
    public static IEndpointRouteBuilder MapUnassignedTickets(
        this IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/dashboard/unassigned-tickets",
            async (
                int? limit,
                ISender sender,
                ICurrentUser currentUser,
                CancellationToken cancellationToken) =>
            {
                var take = Math.Clamp(
                    limit ?? 5,
                    1,
                    20);

                var response = await sender.Send(
                    new GetUnassignedTicketsQuery(
                        currentUser.CompanyId,
                        take),
                    cancellationToken);

                return Results.Ok(response);
            })
            .RequireAuthorization()
            .RequirePermission(Permissions.DashboardView);

        return app;
    }
}