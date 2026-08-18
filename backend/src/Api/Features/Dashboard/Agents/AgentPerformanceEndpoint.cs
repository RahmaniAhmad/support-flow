using Api.Authorization;
using MediatR;
using Shared.Authentication;
using Shared.Domain.Users;


namespace Api.Features.Dashboard.Agents;


public static class AgentPerformanceEndpoint
{

    public static IEndpointRouteBuilder MapAgentPerformance(
        this IEndpointRouteBuilder app)
    {

        app.MapGet(
            "/dashboard/agents",
            async (
                ISender sender,
                ICurrentUser currentUser,
                CancellationToken cancellationToken) =>
            {

                var result =
                    await sender.Send(
                        new GetAgentPerformanceQuery(
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