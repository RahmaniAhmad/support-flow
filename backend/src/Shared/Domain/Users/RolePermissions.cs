namespace Shared.Domain.Users;

public static class RolePermissions
{
    public static readonly Dictionary<UserRole, string[]> Map = new()
    {
        [UserRole.Admin] =
        [
            Permissions.TicketsCreate,
            Permissions.TicketsAssign,
            Permissions.TicketsUnassign,
            Permissions.TicketsResolve,
            Permissions.TicketsReopen,
            Permissions.TicketsClose,
            Permissions.TicketsComment,
            Permissions.TicketsRead,
            Permissions.DashboardView
        ],

        [UserRole.Agent] =
        [
            Permissions.TicketsCreate,
            Permissions.TicketsAssign,
            Permissions.TicketsUnassign,
            Permissions.TicketsResolve,
            Permissions.TicketsClose,
            Permissions.TicketsComment,
            Permissions.DashboardView
        ],

        [UserRole.Customer] =
        [
            Permissions.TicketsCreate,
            Permissions.TicketsComment,
            Permissions.DashboardView
        ]
    };
}