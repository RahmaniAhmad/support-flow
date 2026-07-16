using System.Security.Claims;
using Shared.Domain.Users;

namespace Api.Features.Authentication.Me;

public static class MeEndpoint
{
    public static IEndpointRouteBuilder MapMe(
        this IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/me",
            (ClaimsPrincipal user) =>
            {
                var roleValue = user.FindFirstValue(
                  ClaimTypes.Role);

                if (!Enum.TryParse<UserRole>(
                    roleValue,
                    out var role))
                {
                    return Results.Unauthorized();
                }

                var permissions =
                    RolePermissions.Map.TryGetValue(
                        role,
                        out var rolePermissions)
                        ? rolePermissions
                        : [];

                return Results.Ok(
                    new
                    {
                        UserId = user.FindFirstValue(
                            ClaimTypes.NameIdentifier),

                        Email = user.FindFirstValue(
                            ClaimTypes.Email),

                        Role = user.FindFirstValue(ClaimTypes.Role),

                        CompanyId = user.FindFirstValue(
                            "company_id"),

                        Permissions = permissions

                    });
            })
            .RequireAuthorization();

        return app;
    }
}
