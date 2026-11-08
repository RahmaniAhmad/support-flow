import { useMutation, useQueryClient } from "@tanstack/react-query";
import { queryKeys } from "@/lib/react-query/queryKeys";
import { uploadTicketAttachment } from "../api/attachments";

export function useUploadTicketAttachment() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: uploadTicketAttachment,
    onSuccess: (_attachment, variables) =>
      queryClient.invalidateQueries({
        queryKey: queryKeys.ticketAttachments.list(variables.ticketId),
      }),
  });
}
