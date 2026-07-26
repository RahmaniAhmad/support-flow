import { useQuery } from "@tanstack/react-query";
import { getUsers } from "../services/getUsers";
import { queryKeys } from "@/lib/react-query/queryKeys";

export function useUsers() {
  return useQuery({
    queryKey: queryKeys.users.list(),
    queryFn: getUsers,
  });
}
