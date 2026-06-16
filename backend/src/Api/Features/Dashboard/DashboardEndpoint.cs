using MediatR;
using Shared.Authentication;

namespace Api.Features.Dashboard;

public static class DashboardEndpoint
{
    public static IEndpointRouteBuilder MapDashboard(
        this IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/dashboard",
            async (
                ISender sender,
                ICurrentUser currentUser,
                CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(
                    new DashboardQuery(
                        currentUser.CompanyId),
                        cancellationToken);

                return Results.Ok(response);
            })
            .RequireAuthorization();

        return app;
    }
}
