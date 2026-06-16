using Microsoft.AspNetCore.Authorization;

namespace Api.Authorization;

public sealed class PermissionHandler
    : AuthorizationHandler<PermissionRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        var hasPermission = context.User.Claims
            .Where(c => c.Type == "permission")
            .Any(c => c.Value == requirement.Permission);

        if (hasPermission)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}