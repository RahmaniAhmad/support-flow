import { useQuery } from "@tanstack/react-query";
import { getUnassignedTickets } from "../api";
import { queryKeys } from "@/lib/react-query/queryKeys";

const DEFAULT_LIMIT = 5;

export function useUnassignedTickets(limit = DEFAULT_LIMIT) {
  return useQuery({
    queryKey: queryKeys.dashboard.unassignedTickets(limit),
    queryFn: getUnassignedTickets,

    staleTime: 30_000,
  });
}
