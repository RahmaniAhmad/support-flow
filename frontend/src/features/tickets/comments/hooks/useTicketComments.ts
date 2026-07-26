import { useQuery } from "@tanstack/react-query";

import { queryKeys } from "@/lib/react-query/queryKeys";
import { getTicketComments } from "../api/comment";

export function useTicketComments(ticketId: string) {
  return useQuery({
    queryKey: queryKeys.ticketComments.list(ticketId),

    queryFn: () => getTicketComments(ticketId),

    enabled: !!ticketId,
  });
}
