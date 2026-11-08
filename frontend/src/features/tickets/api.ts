import api from "@/lib/api/axios";
import {
  CreateTicketResponse,
  CreateTicketRequest,
  TicketDetails,
  TicketListFilters,
  TicketListResponse,
} from "./types";
import { TicketPriority } from "@/types/ticket";

export async function getTickets(filters?: TicketListFilters) {
  const { status, ...rest } = filters ?? {};

  const { data } = await api.get<TicketListResponse>("/tickets", {
    params: {
      ...rest,
      status,
    },
  });

  return data;
}

export async function getTicket(id: string) {
  const { data } = await api.get<TicketDetails>(`/tickets/${id}`);

  return data;
}

export async function createTicket({
  request,
  attachments,
}: {
  request: CreateTicketRequest;
  attachments: File[];
}): Promise<CreateTicketResponse> {
  if (attachments.length === 0) {
    const { data } = await api.post<CreateTicketResponse>("/tickets", request);
    return data;
  }

  const formData = new FormData();
  formData.append("subject", request.subject);
  formData.append("description", request.description);
  if (request.categoryId) {
    formData.append("categoryId", request.categoryId);
  }
  attachments.forEach((file) => formData.append("attachments", file));

  const { data } = await api.post<CreateTicketResponse>("/tickets", formData);

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

export async function changeTicketPriority({
  ticketId,
  priority,
}: {
  ticketId: string;
  priority: TicketPriority;
}) {
  await api.put(`/tickets/${ticketId}/priority`, { priority });
}
