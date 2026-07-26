import api from "@/lib/axios";
import { User } from "@/types/user";

export async function getUsers(): Promise<User[]> {
  const response = await api.get<User[]>("/users");

  return response.data;
}
