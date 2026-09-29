"use client";

import { Button, Tooltip } from "antd";
import { MailOutlined } from "@ant-design/icons";

import { AppNotification } from "../types";
import { useMarkAsRead } from "../hooks/useMarkAsRead";

interface Props {
  notification: AppNotification;
}

export function NotificationItem({ notification }: Props) {
  const markAsRead = useMarkAsRead();

  const isUnread = notification.readAtUtc === null;

  const handleMarkAsRead = (e: React.MouseEvent) => {
    e.stopPropagation();

    if (isUnread) {
      markAsRead.mutate(notification.id);
    }
  };

  return (
    <div
      className={`
        flex
        gap-3
        p-3
        hover:bg-gray-50
        ${isUnread ? "bg-blue-50" : ""}
      `}
    >
      <div className="flex-1">
        <div
          className={`
            text-sm
            ${isUnread ? "font-semibold" : "font-medium"}
          `}
        >
          {notification.title}
        </div>

        <div
          className="
            mt-1
            text-sm
            text-gray-600
          "
        >
          {notification.message}
        </div>

        <div
          className="
            mt-2
            text-xs
            text-gray-400
          "
        >
          {new Date(notification.createdAtUtc).toLocaleString()}
        </div>
      </div>

      {isUnread && (
        <Tooltip title="Mark as read">
          <Button
            type="text"
            size="small"
            icon={<MailOutlined />}
            loading={markAsRead.isPending}
            onClick={handleMarkAsRead}
          />
        </Tooltip>
      )}
    </div>
  );
}
