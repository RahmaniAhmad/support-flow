"use client";

import { AppNotification } from "../types";
import { NotificationItem } from "./NotificationItem";

interface Props {
  notifications: AppNotification[];
  isLoading: boolean;
}

export function NotificationList({ notifications, isLoading }: Props) {
  if (isLoading) {
    return (
      <div
        className="
          p-4
          text-center
          text-sm
          text-gray-500
        "
      >
        Loading...
      </div>
    );
  }

  if (notifications.length === 0) {
    return (
      <div
        className="
          p-4
          text-center
          text-sm
          text-gray-500
        "
      >
        No notifications
      </div>
    );
  }

  return (
    <div
      className="
        max-h-[calc(80vh-56px)]
        overflow-y-auto
        divide-y
      "
    >
      {notifications.map((notification) => (
        <NotificationItem key={notification.id} notification={notification} />
      ))}
    </div>
  );
}
