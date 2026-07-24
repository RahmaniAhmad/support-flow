import api from "@/lib/axios";
import { TicketComment } from "@/types/ticketComment";
import { AddCommentRequest } from "../type";

export async function getTicketComments(
  ticketId: string,
): Promise<TicketComment[]> {
  const response = await api.get<TicketComment[]>(
    `/tickets/${ticketId}/comments`,
  );

  return response.data;
}

export async function addComment({
  ticketId,
  request,
}: {
  ticketId: string;
  request: AddCommentRequest;
}): Promise<{ id: string }> {
  const response = await api.post<{ id: string }>(
    `/tickets/${ticketId}/comments`,
    request,
  );

  return response.data;
}
