import { useMutation, useQueryClient } from "@tanstack/react-query";

import { queryKeys } from "@/lib/react-query/queryKeys";
import { resolveTicket } from "../api";

export function useResolveTicket() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: resolveTicket,

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
