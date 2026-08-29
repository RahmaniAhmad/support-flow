import api from "@/lib/api/axios";
import {
  ForgotPasswordRequest,
  LoginRequest,
  RegisterRequest,
  ResetPasswordRequest,
} from "./types";

export async function login(request: LoginRequest) {
  const response = await api.post("/auth/login", request);

  await api.get("/auth/csrf");

  return response.data;
}

export async function register(request: RegisterRequest): Promise<void> {
  await api.post("/auth/register-company", request);
}

export async function logout(): Promise<void> {
  await api.post("/auth/logout");
}

export async function forgotPassword(
  request: ForgotPasswordRequest,
): Promise<void> {
  await api.post("/auth/forgot-password", request);
}

export async function resetPassword(
  request: ResetPasswordRequest,
): Promise<void> {
  await api.post("/auth/reset-password", request);
}
