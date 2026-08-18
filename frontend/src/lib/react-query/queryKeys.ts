import { TicketListFilters } from "@/features/tickets/types";

export const queryKeys = {
  dashboard: {
    all: ["dashboard"] as const,

    statistics: () => [...queryKeys.dashboard.all, "statistics"] as const,

    trend: (from: string, to: string) =>
      [...queryKeys.dashboard.all, "trend", from, to] as const,

    agents: () => [...queryKeys.dashboard.all, "agents"] as const,

    activities: () => [...queryKeys.dashboard.all, "activities"] as const,

    unassignedTickets: (limit: number) =>
      [...queryKeys.dashboard.all, "unassigned-tickets", limit] as const,

    statusDistribution: () =>
      [...queryKeys.dashboard.all, "status-distribution"] as const,
  },

  tickets: {
    all: ["tickets"] as const,

    lists: () => [...queryKeys.tickets.all, "list"] as const,

    list: (filters?: TicketListFilters) =>
      [...queryKeys.tickets.lists(), filters] as const,

    my: (filters?: TicketListFilters) => ["tickets", "my", filters] as const,

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

    details: () => [...queryKeys.users.all, "detail"] as const,

    detail: (id: string) => [...queryKeys.users.details(), id] as const,
  },
  knowledgeArticles: {
    all: ["knowledge-articles"] as const,

    lists: () => [...queryKeys.knowledgeArticles.all, "list"] as const,

    list: () => [...queryKeys.knowledgeArticles.lists()] as const,

    details: () => [...queryKeys.knowledgeArticles.all, "detail"] as const,

    detail: (id: string) =>
      [...queryKeys.knowledgeArticles.details(), id] as const,
  },
  profile: {
    all: ["profile"] as const,

    current: () => [...queryKeys.profile.all, "current"] as const,
  },
} as const;
