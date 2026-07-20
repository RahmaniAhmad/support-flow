import { useQuery } from "@tanstack/react-query";

import { queryKeys } from "@/lib/react-query/queryKeys";
import { TicketListFilters } from "@/types/ticket";

import { getTickets } from "../services/tickets.service";

export function useTickets(filters?: TicketListFilters) {
  return useQuery({
    queryKey: queryKeys.tickets.list(filters),
    queryFn: () => getTickets(filters),
  });
}
