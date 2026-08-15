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
                ISender sender,
                ICurrentUser currentUser,
                CancellationToken cancellationToken) =>
            {


                var result =
                    await sender.Send(
                        new GetRecentActivitiesQuery(
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