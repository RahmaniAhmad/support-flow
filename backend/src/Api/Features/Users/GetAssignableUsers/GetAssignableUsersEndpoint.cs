using Api.Authorization;
using Api.Filters;
using MediatR;
using Shared.Domain.Users;

namespace Api.Features.Users.GetAssignableUsers;

public static class GetAssignableUsersEndpoint
{
    public static IEndpointRouteBuilder MapGetAssignableUsers(
        this IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/users/assignable",
            async (
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var result = await sender.Send(
                    new GetAssignableUsersQuery(),
                    cancellationToken);

                return Results.Ok(result);
            })
            .AddEndpointFilter<SecurityFilter>()
            .RequireAuthorization()
            .RequirePermission(Permissions.TicketsAssign);

        return app;
    }
}