import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query";

import { getAssignableUsers } from "../services/assign.service";
import { queryKeys } from "@/lib/react-query/queryKeys";

export function useAssignableUsers() {
  return useQuery({
    queryKey: queryKeys.users.assignable(),
    queryFn: getAssignableUsers,
  });
}
