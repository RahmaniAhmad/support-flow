import { useMutation, useQueryClient } from "@tanstack/react-query";
import { queryKeys } from "@/lib/react-query/queryKeys";
import { deleteTicketAttachment } from "../api/attachments";

export function useDeleteTicketAttachment(ticketId: string) {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: deleteTicketAttachment,
    onSuccess: () =>
      queryClient.invalidateQueries({
        queryKey: queryKeys.ticketAttachments.list(ticketId),
      }),
  });
}
