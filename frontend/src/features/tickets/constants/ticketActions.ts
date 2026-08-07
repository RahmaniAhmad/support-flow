export const TicketActionKeys = {
  Details: "details",
  Assign: "assign",
  StartProgress: "start-progress",
  MoveToPending: "move-to-pending",
  Resolve: "resolve",
  Close: "close",
  Reopen: "reopen",
  Comment: "comment",
  History: "history",
} as const;

export type TicketAction =
  (typeof TicketActionKeys)[keyof typeof TicketActionKeys];
