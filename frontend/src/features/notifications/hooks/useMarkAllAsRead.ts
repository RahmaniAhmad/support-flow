import { useMutation, useQueryClient } from "@tanstack/react-query";
import { markAllAsRead } from "../api";
import { queryKeys } from "@/lib/react-query/queryKeys";

export function useMarkAllAsRead() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: markAllAsRead,

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
