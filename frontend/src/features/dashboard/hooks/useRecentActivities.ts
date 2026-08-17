import { useQuery } from "@tanstack/react-query";
import { getRecentActivities } from "../api";
import { queryKeys } from "@/lib/react-query/queryKeys";

export function useRecentActivities() {
  return useQuery({
    queryKey: queryKeys.dashboard.activities(),
    queryFn: getRecentActivities,

    staleTime: 30_000,
  });
}
