import { useQuery } from "@tanstack/react-query";
import { getTicketTrend } from "../api";

export function useTicketTrend(from: string, to: string) {
  return useQuery({
    queryKey: ["dashboard", "trend", from, to],

    queryFn: () => getTicketTrend(from, to),

    staleTime: 60_000,
  });
}
