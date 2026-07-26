import api from "@/lib/axios";
import { LoginRequest, RegisterRequest } from "@/types/auth";

export async function login(request: LoginRequest) {
  const response = await api.post("/auth/login", request);

  return response.data;
}

export async function register(request: RegisterRequest): Promise<void> {
  const response = await api.post("/auth/register-company", request);

  return response.data;
}

export async function logout(): Promise<void> {
  await api.post("/auth/logout");
}
