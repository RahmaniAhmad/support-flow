import { useMutation, useQueryClient } from "@tanstack/react-query";

import { addComment } from "../api/comment";
import { queryKeys } from "@/lib/react-query/queryKeys";

export function useAddComment() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: addComment,

    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: queryKeys.ticketComments.list(variables.ticketId),
      });

      queryClient.invalidateQueries({
        queryKey: queryKeys.tickets.detail(variables.ticketId),
      });

      queryClient.invalidateQueries({
        queryKey: queryKeys.tickets.lists(),
      });
    },
  });
}
