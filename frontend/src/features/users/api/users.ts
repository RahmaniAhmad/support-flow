import api from "@/lib/axios";
import {
  CreateUserRequest,
  UpdateUserRequest,
  UserDetails,
  UserListItem,
} from "../types";

export async function getUsers(): Promise<UserListItem[]> {
  const response = await api.get<UserListItem[]>("/users");

  return response.data;
}

export async function getUser(id: string): Promise<UserDetails> {
  const response = await api.get<UserDetails>(`/users/${id}`);

  return response.data;
}

export async function changeUserStatus(id: string, isActive: boolean) {
  await api.put(`/users/${id}/status`, {
    isActive,
  });
}

export async function createUser(data: CreateUserRequest) {
  const response = await api.post("/users", data);

  return response.data;
}

export async function updateUser(id: string, data: UpdateUserRequest) {
  await api.put(`/users/${id}`, data);
}

export async function resetUserPassword(id: string, password: string) {
  await api.put(`/users/${id}/password`, {
    password,
  });
}
