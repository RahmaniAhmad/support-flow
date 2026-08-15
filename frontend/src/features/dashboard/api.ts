import api from "@/lib/axios";
import {
  AgentPerformanceRespons,
  DashboardStatisticsRespons,
  RecentActivityRespons,
  TicketTrendRespons,
} from "./type";

export async function getDashboardStatistics() {
  const response = await api.get<DashboardStatisticsRespons>(
    "/dashboard/statistics",
  );

  return response.data;
}

export async function getTicketTrend(from: string, to: string) {
  const response = await api.get<TicketTrendRespons[]>("/dashboard/trend", {
    params: {
      from,
      to,
    },
  });

  return response.data;
}

export async function getAgentPerformance() {
  const response =
    await api.get<AgentPerformanceRespons[]>("/dashboard/agents");

  return response.data;
}

export async function getRecentActivities() {
  const response = await api.get<RecentActivityRespons[]>(
    "/dashboard/activities",
  );

  return response.data;
}
