import { useQuery } from "@tanstack/react-query";
import { queryKeys } from "@/lib/react-query/queryKeys";
import { getProfile } from "../api";

export function useProfile() {
  return useQuery({
    queryKey: queryKeys.profile.current(),
    queryFn: getProfile,
  });
}
