using MediatR;

namespace Api.Features.Authentication.ForgotPassword;

public static class ForgotPasswordEndpoint
{
    public static IEndpointRouteBuilder MapForgotPassword(
        this IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/auth/forgot-password",
            async (
                ForgotPasswordRequest request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new ForgotPasswordCommand(
                    request.Email);

                await sender.Send(
                    command,
                    cancellationToken);

                return Results.NoContent();
            });

        return app;
    }
}