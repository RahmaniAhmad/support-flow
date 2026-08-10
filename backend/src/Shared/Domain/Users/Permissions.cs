namespace Shared.Domain.Users;

public static class Permissions
{
    // Dashboard
    public const string DashboardView = "dashboard:view";


    // Tickets
    public const string TicketsView = "tickets:view";
    public const string TicketsCreate = "tickets:create";
    public const string TicketsAssign = "tickets:assign";
    public const string TicketsUnassign = "tickets:unassign";
    public const string TicketsStartProgress = "tickets:start_progress";
    public const string TicketsMoveToPending = "tickets:move_to_pending";
    public const string TicketsResolve = "tickets:resolve";
    public const string TicketsReopen = "tickets:reopen";
    public const string TicketsClose = "tickets:close";
    public const string TicketsComment = "tickets:comment";


    // Users
    public const string UsersView = "users:view";
    public const string UsersCreate = "users:create";
    public const string UsersUpdate = "users:update";
    public const string UsersChangeStatus = "users:change-status";
    public const string UsersResetPassword = "users:reset-password";

    // Knowledge Articles
    public const string KnowledgeArticlesView = "knowledge-articles:view";
    public const string KnowledgeArticlesCreate = "knowledge-articles:create";
    public const string KnowledgeArticlesUpdate = "knowledge-articles:update";
    public const string KnowledgeArticlesDelete = "knowledge-articles:delete";

    // AI
    public const string AiSemanticSearch = "ai:semantic-search";
    public const string AiTicketSuggestions = "ai:ticket-suggestions";
}