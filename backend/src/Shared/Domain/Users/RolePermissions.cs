using System.Collections.Immutable;

namespace Shared.Domain.Users;

public static class RolePermissions
{
   public static readonly ImmutableDictionary<UserRole, ImmutableArray<string>> Map =
        new Dictionary<UserRole, string[]>
        {
           [UserRole.SuperAdmin] =
            [
               Permissions.DashboardView,

               Permissions.TicketsView,
               Permissions.TicketsCreate,
               Permissions.TicketsAssign,
               Permissions.TicketsStartProgress,
               Permissions.TicketsMoveToPending,
               Permissions.TicketsResolve,
               Permissions.TicketsReopen,
               Permissions.TicketsClose,
               Permissions.TicketsComment,
               Permissions.UsersView,
               Permissions.UsersCreate,
               Permissions.UsersUpdate,
               Permissions.UsersResetPassword,
               Permissions.UsersChangeStatus,

               Permissions.KnowledgeArticlesView,
               Permissions.KnowledgeArticlesCreate,
               Permissions.KnowledgeArticlesUpdate,
               Permissions.KnowledgeArticlesDelete,

               Permissions.AiSemanticSearch,
               Permissions.AiTicketSuggestions
           ],

           [UserRole.Admin] =
            [
               Permissions.DashboardView,

               Permissions.TicketsView,
               Permissions.TicketsCreate,
               Permissions.TicketsAssign,
               Permissions.TicketsStartProgress,
               Permissions.TicketsMoveToPending,
               Permissions.TicketsResolve,
               Permissions.TicketsReopen,
               Permissions.TicketsClose,
               Permissions.TicketsComment,
               Permissions.UsersView,
               Permissions.UsersCreate,
               Permissions.UsersUpdate,
               Permissions.UsersResetPassword,
               Permissions.UsersChangeStatus,
               Permissions.KnowledgeArticlesView,
               Permissions.KnowledgeArticlesCreate,
               Permissions.KnowledgeArticlesUpdate,
               Permissions.KnowledgeArticlesDelete,

               Permissions.AiSemanticSearch,
               Permissions.AiTicketSuggestions
               ],

           [UserRole.Agent] =
            [
               Permissions.DashboardView,

               Permissions.TicketsView,
               Permissions.TicketsCreate,
               Permissions.TicketsAssign,
               Permissions.TicketsStartProgress,
               Permissions.TicketsMoveToPending,
               Permissions.TicketsResolve,
               Permissions.TicketsClose,
               Permissions.TicketsComment,
               Permissions.KnowledgeArticlesView,
               Permissions.KnowledgeArticlesCreate,
               Permissions.KnowledgeArticlesUpdate,

               Permissions.AiSemanticSearch,
               Permissions.AiTicketSuggestions
               ],

           [UserRole.Customer] =
            [
               Permissions.TicketsView,
               Permissions.TicketsCreate,
               Permissions.TicketsComment,

               Permissions.KnowledgeArticlesView,

               Permissions.AiSemanticSearch,
               Permissions.AiTicketSuggestions
               ]
        }
        .ToImmutableDictionary(x => x.Key, x => x.Value.ToImmutableArray());
}