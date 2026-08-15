import { useQuery } from "@tanstack/react-query";
import { getAgentPerformance } from "../api";

export function useAgentPerformance() {
  return useQuery({
    queryKey: ["dashboard", "agents"],

    queryFn: getAgentPerformance,

    staleTime: 60_000,
  });
}
