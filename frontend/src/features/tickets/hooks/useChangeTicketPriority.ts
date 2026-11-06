import { useMutation, useQueryClient } from "@tanstack/react-query";
import { queryKeys } from "@/lib/react-query/queryKeys";
import { changeTicketPriority } from "../api";

export function useChangeTicketPriority() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: changeTicketPriority,
    onSuccess: (_, variables) => {
      queryClient.invalidateQueries({
        queryKey: queryKeys.tickets.lists(),
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
