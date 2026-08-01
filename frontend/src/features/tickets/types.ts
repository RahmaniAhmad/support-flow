import { TicketPriority, TicketStatus } from "@/types/ticket";

import { PagedResponse, SortDirection } from "@/types/common";

export interface TicketListItem {
  id: string;
  ticketNumber: number;
  subject: string;
  status: TicketStatus;
  priority: TicketPriority;
  companyName?: string;
  assigneeName?: string;
  createdName?: string;
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
  content: string;
  authorUserId: string;
  authorName: string;
  authorEmail: string;
  createdAtUtc: string;
}

export type TicketListResponse = PagedResponse<TicketListItem>;

export type TicketView = "All" | "AssignedToMe" | "CreatedByMe";

export interface TicketListFilters {
  page?: number;
  pageSize?: number;
  search?: string;
  status?: TicketStatus;
  view: TicketView;
  assignedToUserId?: string;
  sortBy?: string;
  sortDirection?: SortDirection;
}

export interface CreateTicketRequest {
  subject: string;
  description: string;
}
