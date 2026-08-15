using MediatR;

namespace Api.Features.Authentication.ResetPassword;

public static class ResetPasswordEndpoint
{
    public static IEndpointRouteBuilder MapResetPassword(
        this IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/auth/reset-password",
            async (
                ResetPasswordRequest request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                await sender.Send(
                    new ResetPasswordCommand(
                        request.Token,
                        request.Password),
                    cancellationToken);

                return Results.NoContent();
            });

        return app;
    }
}