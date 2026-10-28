import api from "@/lib/api/axios";
import {
  TicketCategory,
  TicketCategoryFilters,
  TicketCategoryListResponse,
  TicketCategoryRequest,
} from "../types";

export async function getTicketCategories(
  filters: TicketCategoryFilters,
): Promise<TicketCategoryListResponse> {
  const { data } = await api.get<TicketCategoryListResponse>(
    "/ticket-categories",
    { params: filters },
  );
  return data;
}

export async function getActiveTicketCategories(): Promise<TicketCategory[]> {
  const { data } = await api.get<TicketCategory[]>("/ticket-categories/active");
  return data;
}

export async function getActiveTicketCategoriesForTicket(
  ticketId: string,
): Promise<TicketCategory[]> {
  const { data } = await api.get<TicketCategory[]>(
    `/tickets/${ticketId}/ticket-categories/active`,
  );
  return data;
}

export async function createTicketCategory(
  request: TicketCategoryRequest,
): Promise<{ id: string }> {
  const { data } = await api.post<{ id: string }>(
    "/ticket-categories",
    request,
  );
  return data;
}

export async function updateTicketCategory(
  id: string,
  request: TicketCategoryRequest,
): Promise<void> {
  await api.put(`/ticket-categories/${id}`, request);
}

export async function activateTicketCategory(id: string): Promise<void> {
  await api.put(`/ticket-categories/${id}/activate`);
}

export async function deactivateTicketCategory(id: string): Promise<void> {
  await api.put(`/ticket-categories/${id}/deactivate`);
}

export async function assignTicketCategory(
  ticketId: string,
  categoryId: string,
): Promise<void> {
  await api.put(`/tickets/${ticketId}/category`, { categoryId });
}

export async function removeTicketCategory(ticketId: string): Promise<void> {
  await api.delete(`/tickets/${ticketId}/category`);
}
