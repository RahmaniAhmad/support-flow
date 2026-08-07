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
                Permissions.UsersChangeStatus
               ,
               // Knowledge articles
               Permissions.KnowledgeArticlesView,
               Permissions.KnowledgeArticlesCreate,
               Permissions.KnowledgeArticlesUpdate,
               Permissions.KnowledgeArticlesDelete
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
                // Knowledge articles
                Permissions.KnowledgeArticlesView,
                Permissions.KnowledgeArticlesCreate,
                Permissions.KnowledgeArticlesUpdate,
                Permissions.KnowledgeArticlesDelete,
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
                // Knowledge articles - agents can view and create/update
                Permissions.KnowledgeArticlesView,
                Permissions.KnowledgeArticlesCreate,
                Permissions.KnowledgeArticlesUpdate,
               ],

           [UserRole.Customer] =
            [
               Permissions.TicketsView,
                Permissions.TicketsCreate,
                Permissions.TicketsComment,
               // Customers can view published knowledge articles via API
               Permissions.KnowledgeArticlesView
               ]
        }
        .ToImmutableDictionary(x => x.Key, x => x.Value.ToImmutableArray());
}