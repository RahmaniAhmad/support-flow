import { useQuery } from "@tanstack/react-query";

import { getDashboard } from "../services/dashboard.service";
import { queryKeys } from "@/lib/react-query/queryKeys";

export function useDashboard() {
  return useQuery({
    queryKey: queryKeys.dashboard.all,
    queryFn: getDashboard,
    staleTime: 60000,
  });
}
