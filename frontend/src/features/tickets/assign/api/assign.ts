import api from "@/lib/api/axios";
import { AssignableUser, AssignTicketRequest } from "../types";

export async function getAssignableUsers(): Promise<AssignableUser[]> {
  const response = await api.get<AssignableUser[]>(`/users/assignable`);

  return response.data;
}

export async function assignTicket({
  ticketId,
  request,
}: {
  ticketId: string;
  request: AssignTicketRequest;
}): Promise<{ id: string }> {
  const response = await api.put<{ id: string }>(
    `/tickets/${ticketId}/assign`,
    request,
  );

  return response.data;
}
