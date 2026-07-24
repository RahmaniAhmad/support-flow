import { useMutation, useQueryClient } from "@tanstack/react-query";

import { addComment } from "../services/comment.service";
import { queryKeys } from "@/lib/react-query/queryKeys";

export function useAddComment(ticketId: string) {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (content: string) => addComment(ticketId, content),

    onSuccess: () => {
      queryClient.invalidateQueries({
        queryKey: queryKeys.tickets.comments(ticketId),
      });

      queryClient.invalidateQueries({
        queryKey: queryKeys.tickets.detail(ticketId),
      });
    },
  });
}
