import { useQuery } from "@tanstack/react-query";
import { getProfile } from "../api/profile";
import { queryKeys } from "@/lib/react-query/queryKeys";

export function useProfile() {
  return useQuery({
    queryKey: queryKeys.profile.current(),
    queryFn: getProfile,
  });
}
