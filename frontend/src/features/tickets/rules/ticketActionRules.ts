import { TicketStatus } from "@/types/ticket";

export function canAssignTicket(status: TicketStatus) {
  return status === "Open" || status === "Reopened";
}

export function canStartProgress(status: TicketStatus) {
  return status === "Assigned" || status === "Reopened";
}

export function canResolveTicket(status: TicketStatus) {
  return status === "InProgress";
}

export function canCloseTicket(status: TicketStatus) {
  return status !== "Closed";
}

export function canReopenTicket(status: TicketStatus) {
  return status === "Resolved" || status === "Closed";
}

export function canAddComment(status: TicketStatus) {
  return status !== "Closed";
}
