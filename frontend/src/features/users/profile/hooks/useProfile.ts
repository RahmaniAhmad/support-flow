import { useQuery } from "@tanstack/react-query";
import { getProfile } from "../services/profile.service";
import { queryKeys } from "@/lib/react-query/queryKeys";

export function useProfile() {
  return useQuery({
    queryKey: queryKeys.users.profile(),
    queryFn: getProfile,
  });
}
