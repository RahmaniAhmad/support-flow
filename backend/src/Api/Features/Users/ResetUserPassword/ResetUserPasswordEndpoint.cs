using Api.Authorization;
using MediatR;
using Shared.Domain.Users;

namespace Api.Features.Users.ResetUserPassword;

public static class ResetUserPasswordEndpoint
{
    public static IEndpointRouteBuilder MapResetUserPassword(
        this IEndpointRouteBuilder app)
    {
        app.MapPut(
            "/users/{id:guid}/password",
            async (
                Guid id,
                ResetUserPasswordRequest request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                await sender.Send(
                    new ResetUserPasswordCommand(
                        id,
                        request.Password),
                    cancellationToken);


                return Results.NoContent();
            })
            .RequireAuthorization()
            .RequirePermission(Permissions.UsersResetPassword);


        return app;
    }
}