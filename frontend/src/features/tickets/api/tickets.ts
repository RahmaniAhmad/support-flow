import api from "@/lib/axios";
import {
  CreateTicketRequest,
  TicketDetails,
  TicketListFilters,
  TicketListResponse,
} from "../types";

export async function getTickets(filters?: TicketListFilters) {
  const { data } = await api.get<TicketListResponse>("/tickets", {
    params: filters,
  });

  return data;
}

export async function getTicket(id: string) {
  const { data } = await api.get<TicketDetails>(`/tickets/${id}`);

  return data;
}

export async function createTicket(request: CreateTicketRequest) {
  const { data } = await api.post<TicketDetails>("/tickets", request);

  return data;
}

export async function startProgressTicket(ticketId: string) {
  await api.post(`/tickets/${ticketId}/start-progress`);
}

export async function resolveTicket(ticketId: string) {
  const { data } = await api.post(`/tickets/${ticketId}/resolve`);

  return data;
}

export async function reopenTicket(ticketId: string) {
  const { data } = await api.post(`/tickets/${ticketId}/reopen`);

  return data;
}

export async function closeTicket(ticketId: string) {
  const { data } = await api.post(`/tickets/${ticketId}/close`);

  return data;
}

export async function moveTicketToPending(ticketId: string) {
  const { data } = await api.post(`/tickets/${ticketId}/move-to-pending`);

  return data;
}
