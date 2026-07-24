import { useQuery } from "@tanstack/react-query";

import { queryKeys } from "@/lib/react-query/queryKeys";

import { getTickets } from "../services/tickets.service";
import { TicketListFilters } from "@/types/ticketFilters";

export function useTickets(filters?: TicketListFilters) {
  return useQuery({
    queryKey: queryKeys.tickets.list(filters),
    queryFn: () => getTickets(filters),
  });
}
