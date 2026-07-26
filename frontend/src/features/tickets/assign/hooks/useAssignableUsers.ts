import { useQuery } from "@tanstack/react-query";
import { getAssignableUsers } from "../api/assign";
import { queryKeys } from "@/lib/react-query/queryKeys";

export function useAssignableUsers() {
  return useQuery({
    queryKey: queryKeys.ticketAssignment.assignableUsers(),
    queryFn: getAssignableUsers,
  });
}
