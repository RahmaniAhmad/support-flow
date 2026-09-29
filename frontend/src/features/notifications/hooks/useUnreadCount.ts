import { useQuery } from "@tanstack/react-query";
import { getUnreadCount } from "../api";
import { queryKeys } from "@/lib/react-query/queryKeys";

export function useUnreadCount() {
  return useQuery({
    queryKey: queryKeys.notifications.unreadCount(),
    queryFn: getUnreadCount,
  });
}
