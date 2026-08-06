import { useMutation, useQueryClient } from "@tanstack/react-query";

import { queryKeys } from "@/lib/react-query/queryKeys";
import { startProgressTicket } from "../api/tickets";

export function useStartProgressTicket() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: startProgressTicket,

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
