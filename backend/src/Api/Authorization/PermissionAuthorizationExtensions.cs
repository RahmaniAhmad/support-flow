namespace Api.Authorization;

public static class PermissionAuthorizationExtensions
{
    public static IEndpointConventionBuilder RequirePermission(
        this IEndpointConventionBuilder builder,
        string permission)
    {
        return builder.RequireAuthorization(
            policy =>
                policy.AddRequirements(
                    new PermissionRequirement(permission)));
    }
}