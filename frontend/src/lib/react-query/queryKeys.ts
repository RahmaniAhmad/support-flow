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
  },

  ticketComments: {
    all: ["ticket-comments"] as const,

    list: (ticketId: string) =>
      [...queryKeys.ticketComments.all, ticketId] as const,
  },

  ticketAssignment: {
    all: ["ticket-assignment"] as const,

    assignableUsers: () =>
      [...queryKeys.ticketAssignment.all, "assignable-users"] as const,
  },

  users: {
    all: ["users"] as const,

    lists: () => [...queryKeys.users.all, "list"] as const,

    list: (filters?: unknown) => [...queryKeys.users.lists(), filters] as const,
  },
  profile: {
    all: ["profile"] as const,

    current: () => [...queryKeys.profile.all, "current"] as const,
  },
} as const;
