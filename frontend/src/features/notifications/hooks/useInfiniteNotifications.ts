import { useInfiniteQuery } from "@tanstack/react-query";

import { getNotifications } from "../api";
import { queryKeys } from "@/lib/react-query/queryKeys";

const PAGE_SIZE = 5;

export function useInfiniteNotifications() {
  return useInfiniteQuery({
    queryKey: queryKeys.notifications.list(),

    queryFn: ({ pageParam = 1 }) => getNotifications(pageParam, PAGE_SIZE),

    initialPageParam: 1,

    getNextPageParam: (lastPage, allPages) => {
      const loadedCount = allPages.reduce(
        (total, page) => total + page.items.length,
        0,
      );

      if (loadedCount >= lastPage.totalCount) {
        return undefined;
      }

      return lastPage.page + 1;
    },
  });
}
