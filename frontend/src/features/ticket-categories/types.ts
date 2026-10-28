import { PagedResponse } from "@/types/common";

export interface TicketCategory {
  id: string;
  name: string;
  description?: string;
  isActive: boolean;
  createdAtUtc: string;
  updatedAtUtc?: string;
}

export interface TicketCategoryFilters {
  page: number;
  pageSize: number;
  search?: string;
}

export interface TicketCategoryRequest {
  name: string;
  description?: string;
}

export type TicketCategoryListResponse = PagedResponse<TicketCategory>;
