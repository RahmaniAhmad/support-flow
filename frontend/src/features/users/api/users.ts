import api from "@/lib/axios";
import { UserListItem } from "../types";

export async function getUsers(): Promise<UserListItem[]> {
  const response = await api.get<UserListItem[]>("/users");

  return response.data;
}

export async function changeUserStatus(id: string, isActive: boolean) {
  await api.put(`/users/${id}/status`, {
    isActive,
  });
}
