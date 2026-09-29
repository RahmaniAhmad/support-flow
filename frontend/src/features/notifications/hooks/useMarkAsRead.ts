import { useMutation, useQueryClient } from "@tanstack/react-query";
import { markAsRead } from "../api";
import { queryKeys } from "@/lib/react-query/queryKeys";

export function useMarkAsRead() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: markAsRead,

    onSuccess: () => {
      queryClient.invalidateQueries({
        queryKey: queryKeys.notifications.list(),
      });

      queryClient.invalidateQueries({
        queryKey: queryKeys.notifications.unreadCount(),
      });
    },
  });
}
