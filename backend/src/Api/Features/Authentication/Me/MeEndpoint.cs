using System.Security.Claims;

namespace SupportFlow.Api.Features.Authentication.Me
{

    public static class MeEndpoint
    {
        public static IEndpointRouteBuilder MapMe(
            this IEndpointRouteBuilder app)
        {
            app.MapGet(
                "/me",
                (ClaimsPrincipal user) =>
                {
                    return Results.Ok(
                        new
                        {
                            UserId = user.FindFirstValue(
                                ClaimTypes.NameIdentifier),

                            Email = user.FindFirstValue(
                                ClaimTypes.Email),

                            CompanyId = user.FindFirstValue(
                                "company_id")
                        });
                })
                .RequireAuthorization();

            return app;
        }
    }
}