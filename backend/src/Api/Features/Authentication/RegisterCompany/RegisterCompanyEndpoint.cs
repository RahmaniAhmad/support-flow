using MediatR;
namespace Api.Features.Authentication.RegisterCompany;

public static class RegisterCompanyEndpoint
{
    public static IEndpointRouteBuilder MapRegisterCompany(
        this IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/auth/register",
            async (
                RegisterCompanyRequest request,
                ISender sender,
                CancellationToken cancellationToken) =>
            {
                var command = new RegisterCompanyCommand
                (
                    request.CompanyName,
                    request.Email,
                    request.Password
                );
                var result = await sender.Send(command, cancellationToken);

                return Results.Ok(result);
            });

        return app;
    }
}
