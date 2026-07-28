import api from "@/lib/axios";
import { DashboardResponse } from "../type";

export async function getDashboard() {
  const response = await api.get<DashboardResponse>("/dashboard");

  return response.data;
}
