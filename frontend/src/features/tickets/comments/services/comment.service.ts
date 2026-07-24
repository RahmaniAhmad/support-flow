import api from "@/lib/axios";
import { TicketComment } from "../types";

export async function getTicketComments(
  ticketId: string,
): Promise<TicketComment[]> {
  const response = await api.get<TicketComment[]>(
    `/tickets/${ticketId}/comments`,
  );

  return response.data;
}

export async function addComment(
  ticketId: string,
  content: string,
): Promise<{ id: string }> {
  const response = await api.post<{ id: string }>(
    `/tickets/${ticketId}/comments`,
    {
      content,
    },
  );

  return response.data;
}
