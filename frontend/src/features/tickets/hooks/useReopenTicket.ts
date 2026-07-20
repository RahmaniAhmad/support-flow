import { useMutation, useQueryClient } from "@tanstack/react-query";

import { reopenTicket } from "../services/tickets.service";
import { queryKeys } from "@/lib/react-query/queryKeys";

export function useReopenTicket() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: reopenTicket,

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
