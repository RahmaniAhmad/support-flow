// Ticket Status

import { PagedResponse, SortDirection } from "./common";

export type TicketStatus =
  | "Open"
  | "Assigned"
  | "InProgress"
  | "Resolved"
  | "Reopened"
  | "Closed";

// Ticket Priority

export type TicketPriority = "Low" | "Medium" | "High" | "Critical";

// -----------------------------------------------------
// Query Models (API Responses)
// -----------------------------------------------------

export interface TicketSummary {
  id: string;
  ticketNumber: number;
  subject: string;
  status: TicketStatus;
  priority: TicketPriority;
  assigneeId?: string;
  assigneeName?: string;
  createdAtUtc: string;
}

export interface TicketDetails {
  id: string;
  ticketNumber: number;
  companyId: string;
  createdByUserId: string;
  createdByName: string;
  assignedToUserId?: string;
  assigneeName?: string;
  subject: string;
  description: string;
  status: TicketStatus;
  priority: TicketPriority;
  createdAtUtc: string;
  updatedAtUtc?: string;
  comments: TicketComment[];
}

export interface TicketComment {
  id: string;
  authorUserId: string;
  authorName: string;
  content: string;
  createdAtUtc: string;
}

// -----------------------------------------------------
// List Query
// -----------------------------------------------------

export interface TicketListFilters {
  page?: number;
  pageSize?: number;
  search?: string;
  status?: TicketStatus;
  priority?: TicketPriority;
  assignedToUserId?: string;
  sortBy?: string;
  sortDirection?: SortDirection;
}

export type TicketListResponse = PagedResponse<TicketSummary>;

// -----------------------------------------------------
// Command Models (API Requests)
// -----------------------------------------------------

export interface CreateTicketRequest {
  subject: string;
  description: string;
}

export interface AssignTicketRequest {
  assignedToUserId: string;
}

export interface AddCommentRequest {
  content: string;
}
