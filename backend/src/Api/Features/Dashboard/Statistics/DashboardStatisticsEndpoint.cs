using Api.Authorization;
using MediatR;
using Shared.Authentication;
using Shared.Domain.Users;

namespace Api.Features.Dashboard.Statistics;


public static class DashboardStatisticsEndpoint
{

    public static IEndpointRouteBuilder MapDashboardStatistics(
        this IEndpointRouteBuilder app)
    {

        app.MapGet(
            "/dashboard/statistics",
            async (
                ISender sender,
                ICurrentUser currentUser,
                CancellationToken cancellationToken) =>
            {

                var result = await sender.Send(
                    new GetDashboardStatisticsQuery(
                        currentUser.CompanyId),
                    cancellationToken);


                return Results.Ok(result);

            })
            .RequireAuthorization()
            .RequirePermission(
                Permissions.DashboardView);


        return app;
    }

}