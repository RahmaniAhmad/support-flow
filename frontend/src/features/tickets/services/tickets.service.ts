import api from "@/lib/axios";
import {
  AddCommentRequest,
  AssignTicketRequest,
  CreateTicketRequest,
  TicketDetails,
  TicketListFilters,
  TicketListResponse,
} from "@/types/ticket";

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

export async function assignTicket(
  ticketId: string,
  request: AssignTicketRequest,
) {
  const { data } = await api.post(`/tickets/${ticketId}/assign`, request);

  return data;
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

export async function addComment(ticketId: string, request: AddCommentRequest) {
  const { data } = await api.post(`/tickets/${ticketId}/comments`, request);

  return data;
}
