import { useQuery } from "@tanstack/react-query";
import { getRecentActivities } from "../api";

export function useRecentActivities() {
  return useQuery({
    queryKey: ["dashboard", "activities"],

    queryFn: getRecentActivities,

    staleTime: 30_000,
  });
}
