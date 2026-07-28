export const AppPermissions = {
  TicketsCreate: "tickets:create",
  TicketsAssign: "tickets:assign",
  TicketsUnassign: "tickets:unassign",
  TicketsStartProgress: "tickets:start_progress",
  TicketsResolve: "tickets:resolve",
  TicketsReopen: "tickets:reopen",
  TicketsClose: "tickets:close",
  TicketsComment: "tickets:comment",
  TicketsRead: "tickets:read",

  DashboardView: "dashboard:view",

  UsersView: "users:view",
  UsersCreate: "users:create",
  UsersUpdate: "users:update",
  UsersChangeRole: "users:change-role",
  UsersResetPassword: "users:reset-password",
} as const;

export type Permission = (typeof AppPermissions)[keyof typeof AppPermissions];
