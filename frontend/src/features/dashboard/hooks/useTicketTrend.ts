import { useQuery } from "@tanstack/react-query";
import { getTicketTrend } from "../api";
import { queryKeys } from "@/lib/react-query/queryKeys";

export function useTicketTrend(from: string, to: string) {
  return useQuery({
    queryKey: queryKeys.dashboard.trend(from, to),
    queryFn: () => getTicketTrend(from, to),

    staleTime: 60_000,
  });
}
