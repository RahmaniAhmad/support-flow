"use client";

import { useInfiniteNotifications } from "../hooks";
import { LoadMoreButton } from "./LoadMoreButton";
import { NotificationHeader } from "./NotificationHeader";
import { NotificationList } from "./NotificationList";

export function NotificationPanel() {
  const { data, isLoading, fetchNextPage, hasNextPage, isFetchingNextPage } =
    useInfiniteNotifications();

  const notifications = data?.pages.flatMap((page) => page.items) ?? [];
  return (
    <div
      className="
        w-[calc(100vw)]
        md:max-w-96
        max-h-[90vh]
        overflow-hidden
        rounded-lg
        bg-white
        shadow-2xl
        md:mr-2
      "
    >
      <NotificationHeader notifications={notifications} />

      <NotificationList notifications={notifications} isLoading={isLoading} />

      <LoadMoreButton
        hasMore={!!hasNextPage}
        loading={isFetchingNextPage}
        onClick={() => fetchNextPage()}
      />
    </div>
  );
}
