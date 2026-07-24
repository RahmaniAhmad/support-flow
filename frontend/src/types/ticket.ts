import { TicketPriority, TicketStatus } from "./ticketEnums";
import { TicketComment } from "./ticketComment";
import { PagedResponse } from "./common";

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

export type TicketListResponse = PagedResponse<TicketSummary>;
