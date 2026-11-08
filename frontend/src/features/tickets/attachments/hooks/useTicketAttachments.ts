import { useQuery } from "@tanstack/react-query";
import { queryKeys } from "@/lib/react-query/queryKeys";
import { getTicketAttachments } from "../api/attachments";

export function useTicketAttachments(ticketId: string) {
  return useQuery({
    queryKey: queryKeys.ticketAttachments.list(ticketId),
    queryFn: () => getTicketAttachments(ticketId),
    enabled: !!ticketId,
  });
}
