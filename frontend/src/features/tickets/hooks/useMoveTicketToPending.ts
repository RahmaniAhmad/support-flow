import { useMutation, useQueryClient } from "@tanstack/react-query";

import { moveTicketToPending } from "../api";
import { queryKeys } from "@/lib/react-query/queryKeys";

export function useMoveTicketToPending() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: moveTicketToPending,

    onSuccess: () => {
      queryClient.invalidateQueries({
        queryKey: queryKeys.tickets.all,
      });

      queryClient.invalidateQueries({
        queryKey: queryKeys.dashboard.all,
      });
    },
  });
}
