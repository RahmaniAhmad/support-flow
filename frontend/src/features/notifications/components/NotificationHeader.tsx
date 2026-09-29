"use client";

import { AppNotification } from "../types";
import { MarkAllReadButton } from "./MarkAllReadButton";

interface Props {
  notifications: AppNotification[];
}

export function NotificationHeader({ notifications }: Props) {
  const hasUnread = notifications.some((x) => x.readAtUtc === null);

  return (
    <div
      className="
        flex
        items-center
        justify-between
        border-b
        bg-slate-800
        px-4
        py-3
      "
    >
      <h3
        className="
          font-semibold
          text-white
        "
      >
        Notifications
      </h3>

      {hasUnread && <MarkAllReadButton />}
    </div>
  );
}
