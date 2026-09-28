"use client";

import { useNotifications } from "../hooks/useNotificationHub";

export function NotificationDropdown() {
  const { notifications } = useNotifications();

  return (
    <div
      className="
        w-96
        rounded-lg
        bg-background
        shadow-lg
      "
    >
      {notifications.length === 0 ? (
        <div className="p-4 text-center text-sm text-gray-500">
          No notifications
        </div>
      ) : (
        <div className="divide-y">
          {notifications.map((notification) => (
            <div
              key={notification.id}
              className="cursor-pointer p-3 hover:bg-gray-50"
            >
              <div className="font-medium">{notification.title}</div>

              <div className="text-sm text-gray-600">
                {notification.message}
              </div>

              <div className="mt-1 text-xs text-gray-400">
                {new Date(notification.createdAtUtc).toLocaleString()}
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}
