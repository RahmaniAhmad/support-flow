import { useMutation, useQueryClient } from "@tanstack/react-query";

import { createTicket } from "../services/tickets.service";
import { queryKeys } from "@/lib/react-query/queryKeys";

export function useCreateTicket() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: createTicket,

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
