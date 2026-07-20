import { useMutation, useQueryClient } from "@tanstack/react-query";

import { closeTicket } from "../services/tickets.service";
import { queryKeys } from "@/lib/react-query/queryKeys";

export function useCloseTicket() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: closeTicket,

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
