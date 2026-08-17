import { useQuery } from "@tanstack/react-query";
import { getDashboardStatistics } from "../api";
import { queryKeys } from "@/lib/react-query/queryKeys";

export function useDashboardStatistics() {
  return useQuery({
    queryKey: queryKeys.dashboard.statistics(),
    queryFn: getDashboardStatistics,

    staleTime: 60_000,
  });
}
