import api from "@/lib/axios";
import {
  ForgotPasswordRequest,
  LoginRequest,
  RegisterRequest,
  ResetPasswordRequest,
} from "../types";

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

export async function forgotPassword(
  request: ForgotPasswordRequest,
): Promise<void> {
  const response = await api.post("/auth/forgot-password", request);

  return response.data;
}

export async function resetPassword(
  request: ResetPasswordRequest,
): Promise<void> {
  const response = await api.post("/auth/reset-password", request);

  return response.data;
}
