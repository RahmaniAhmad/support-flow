import { useMutation, useQueryClient } from "@tanstack/react-query";

import { queryKeys } from "@/lib/react-query/queryKeys";
import { assignTicket } from "../services/assign.service";

export function useAssignTicket() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: assignTicket,

    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: queryKeys.tickets.list(),
      });

      queryClient.invalidateQueries({
        queryKey: queryKeys.tickets.detail(variables.ticketId),
      });

      queryClient.invalidateQueries({
        queryKey: queryKeys.dashboard.all,
      });
    },
  });
}
