import {
  TicketFilter,
  TicketStatusParam,
  TicketView,
  TicketViewParam,
} from "../types";

export const ticketStatusParamMap: Record<TicketStatusParam, TicketFilter> = {
  open: "Open",
  assigned: "Assigned",
  "in-progress": "InProgress",
  pending: "Pending",
  resolved: "Resolved",
  reopened: "Reopened",
  closed: "Closed",
  unassigned: "Unassigned",
};

export const ticketStatusUrlMap: Record<TicketFilter, TicketStatusParam> = {
  Open: "open",
  Assigned: "assigned",
  InProgress: "in-progress",
  Pending: "pending",
  Resolved: "resolved",
  Reopened: "reopened",
  Closed: "closed",
  Unassigned: "unassigned",
};

export const ticketViewParamMap: Record<TicketViewParam, TicketView> = {
  all: "All",
  "assigned-to-me": "AssignedToMe",
  "created-by-me": "CreatedByMe",
};

export const ticketViewUrlMap: Record<TicketView, TicketViewParam> = {
  All: "all",
  AssignedToMe: "assigned-to-me",
  CreatedByMe: "created-by-me",
};
