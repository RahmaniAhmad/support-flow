export const AppPermissions = {
  // Dashboard
  DashboardView: "dashboard:view",

  // Tickets
  TicketsView: "tickets:view",
  TicketsCreate: "tickets:create",
  TicketsAssign: "tickets:assign",
  TicketsUnassign: "tickets:unassign",
  TicketsStartProgress: "tickets:start_progress",
  TicketsResolve: "tickets:resolve",
  TicketsReopen: "tickets:reopen",
  TicketsClose: "tickets:close",
  TicketsComment: "tickets:comment",

  // Users
  UsersView: "users:view",
  UsersCreate: "users:create",
  UsersUpdate: "users:update",
  UsersChangeStatus: "users:change-status",
  UsersResetPassword: "users:reset-password",
} as const;

export type Permission = (typeof AppPermissions)[keyof typeof AppPermissions];
