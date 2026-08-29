import { useQuery } from "@tanstack/react-query";
import { queryKeys } from "@/lib/react-query/queryKeys";
import { getTicket } from "../api";

export function useTicket(id: string) {
  return useQuery({
    queryKey: queryKeys.tickets.detail(id),
    queryFn: () => getTicket(id),
    enabled: !!id,
  });
}
