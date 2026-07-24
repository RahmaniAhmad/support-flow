import { useQuery } from "@tanstack/react-query";

import { queryKeys } from "@/lib/react-query/queryKeys";
import { getTicketComments } from "../services/comment.service";

export function useTicketComments(ticketId: string) {
  return useQuery({
    queryKey: queryKeys.tickets.comments(ticketId),

    queryFn: () => getTicketComments(ticketId),

    enabled: !!ticketId,
  });
}
