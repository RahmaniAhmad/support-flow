namespace Shared.Domain.Users;

public static class RolePermissions
{
    public static readonly Dictionary<UserRole, string[]> Map = new()
    {
        [UserRole.SuperAdmin] =
        [
            Permissions.TicketsCreate,
            Permissions.TicketsAssign,
            Permissions.TicketsUnassign,
            Permissions.TicketsStartProgress,
            Permissions.TicketsResolve,
            Permissions.TicketsReopen,
            Permissions.TicketsClose,
            Permissions.TicketsComment,
            Permissions.TicketsRead,

            Permissions.DashboardView,

            Permissions.UsersView,
            Permissions.UsersCreate,
            Permissions.UsersUpdate,
            Permissions.UsersChangeRole,
            Permissions.UsersResetPassword
        ],

        [UserRole.Admin] =
        [
            Permissions.TicketsCreate,
            Permissions.TicketsAssign,
            Permissions.TicketsUnassign,
            Permissions.TicketsStartProgress,
            Permissions.TicketsResolve,
            Permissions.TicketsReopen,
            Permissions.TicketsClose,
            Permissions.TicketsComment,
            Permissions.TicketsRead,

            Permissions.DashboardView,

            Permissions.UsersView,
            Permissions.UsersCreate,
            Permissions.UsersUpdate,
            Permissions.UsersChangeRole,
            Permissions.UsersResetPassword
        ],

        [UserRole.Agent] =
        [
            Permissions.TicketsCreate,
            Permissions.TicketsAssign,
            Permissions.TicketsStartProgress,
            Permissions.TicketsResolve,
            Permissions.TicketsClose,
            Permissions.TicketsComment,
            Permissions.TicketsRead,

            Permissions.DashboardView
        ],

        [UserRole.Customer] =
        [
            Permissions.TicketsCreate,
            Permissions.TicketsComment,
            Permissions.TicketsRead
        ]
    };
}