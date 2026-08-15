import { useQuery } from "@tanstack/react-query";
import { getDashboardStatistics } from "../api";

export function useDashboardStatistics() {
  return useQuery({
    queryKey: ["dashboard", "statistics"],

    queryFn: getDashboardStatistics,

    staleTime: 60_000,
  });
}
