using Api.Authorization;
using MediatR;
using Shared.Authentication;
using Shared.Domain.Users;


namespace Api.Features.Dashboard.Activity;


public static class RecentActivitiesEndpoint
{

    public static IEndpointRouteBuilder MapRecentActivities(
        this IEndpointRouteBuilder app)
    {


        app.MapGet(
            "/dashboard/activities",
            async (
                int? limit,
                ISender sender,
                ICurrentUser currentUser,
                CancellationToken cancellationToken) =>
            {


                var result =
                    await sender.Send(
                        new GetRecentActivitiesQuery(
                            currentUser.CompanyId,
                            Math.Clamp(limit ?? 5, 1, 20)),
                        cancellationToken);



                return Results.Ok(result);


            })
            .RequireAuthorization()
            .RequirePermission(
                Permissions.DashboardView);



        return app;
    }

}