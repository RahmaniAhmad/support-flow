namespace Shared.Domain.Users;

public static class Permissions
{
    public const string TicketsCreate = "tickets:create";
    public const string TicketsAssign = "tickets:assign";
    public const string TicketsUnassign = "tickets:unassign";
    public const string TicketsStartProgress = "tickets:start_progress";
    public const string TicketsResolve = "tickets:resolve";
    public const string TicketsReopen = "tickets:reopen";
    public const string TicketsClose = "tickets:close";
    public const string TicketsComment = "tickets:comment";

    public const string TicketsRead = "tickets:read";

    public const string DashboardView = "dashboard:view";
}