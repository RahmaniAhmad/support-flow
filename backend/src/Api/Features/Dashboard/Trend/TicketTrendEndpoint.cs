using Api.Authorization;
using MediatR;
using Shared.Authentication;
using Shared.Domain.Users;


namespace Api.Features.Dashboard.Trend;


public static class TicketTrendEndpoint
{

    public static IEndpointRouteBuilder MapTicketTrend(
    this IEndpointRouteBuilder app)
    {


        app.MapGet(
        "/dashboard/trend",
        async (
        ISender sender,
        ICurrentUser currentUser,
        DateOnly from,
        DateOnly to,
        CancellationToken cancellationToken) =>
        {


            var result = await sender.Send(
    new GetTicketTrendQuery(
    currentUser.CompanyId,
    from,
    to),
    cancellationToken);


            return Results.Ok(result);


        })
        .RequireAuthorization()
        .RequirePermission(
        Permissions.DashboardView);



        return app;

    }

}