import { useQuery } from "@tanstack/react-query";
import { getUsers } from "../api/users";
import { queryKeys } from "@/lib/react-query/queryKeys";
import { UserListFilters } from "../types";

export function useUsers(filters?: UserListFilters) {
  return useQuery({
    queryKey: queryKeys.users.list(filters),
    queryFn: getUsers,
  });
}
