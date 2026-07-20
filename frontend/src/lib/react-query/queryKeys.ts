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
} as const;
