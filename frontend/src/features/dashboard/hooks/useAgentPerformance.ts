import { useQuery } from "@tanstack/react-query";
import { getAgentPerformance } from "../api";
import { queryKeys } from "@/lib/react-query/queryKeys";

export function useAgentPerformance() {
  return useQuery({
    queryKey: queryKeys.dashboard.agents(),
    queryFn: getAgentPerformance,

    staleTime: 30_000,
  });
}
