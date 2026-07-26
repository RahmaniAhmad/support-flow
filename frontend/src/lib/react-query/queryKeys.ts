export const queryKeys = {
  dashboard: {
    all: ["dashboard"] as const,
  },

  tickets: {
    all: ["tickets"] as const,

    lists: () => [...queryKeys.tickets.all, "list"] as const,

    list: (filters?: unknown) =>
      [...queryKeys.tickets.lists(), filters] as const,

    details: () => [...queryKeys.tickets.all, "detail"] as const,

    detail: (id: string) => [...queryKeys.tickets.details(), id] as const,

    comments: (ticketId: string) =>
      [...queryKeys.tickets.all, ticketId, "comments"] as const,

    assign: (ticketId: string) =>
      [...queryKeys.tickets.all, ticketId, "assign"] as const,
  },

  users: {
    all: ["users"] as const,

    lists: () => [...queryKeys.users.all, "list"] as const,

    list: () => [...queryKeys.users.lists()] as const,

    profile: () => [...queryKeys.users.all, "profile"] as const,

    assignable: () => [...queryKeys.users.all, "assignable"] as const,
  },
} as const;
