import { SortDirection } from "@/types/common";
import { TicketPriority, TicketStatus } from "@/types/ticketEnums";

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

export interface CreateTicketRequest {
  subject: string;
  description: string;
}
