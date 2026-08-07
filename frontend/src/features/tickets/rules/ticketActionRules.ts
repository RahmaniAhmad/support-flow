import { TicketListItem } from "../types";

export function canAssignTicket(ticket: TicketListItem): boolean {
  return ticket.status === "Open" || ticket.status === "Reopened";
}

export function canStartProgress(ticket: TicketListItem): boolean {
  return (
    !!ticket.assignedToUserId &&
    (ticket.status === "Assigned" ||
      ticket.status === "Reopened" ||
      ticket.status === "Pending")
  );
}

export function canResolveTicket(ticket: TicketListItem): boolean {
  return ticket.status === "InProgress";
}

export function canMoveToPending(ticket: TicketListItem): boolean {
  return ticket.status === "InProgress";
}

export function canCloseTicket(ticket: TicketListItem): boolean {
  return ticket.status !== "Closed";
}

export function canReopenTicket(ticket: TicketListItem): boolean {
  return ticket.status === "Resolved" || ticket.status === "Closed";
}

export function canAddComment(ticket: TicketListItem): boolean {
  return ticket.status !== "Closed";
}
