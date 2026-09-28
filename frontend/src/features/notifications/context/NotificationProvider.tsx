"use client";

import { HubConnectionBuilder, LogLevel } from "@microsoft/signalr";

import {
  createContext,
  ReactNode,
  useCallback,
  useEffect,
  useState,
} from "react";

import { AppNotification } from "@/features/notifications/types";

interface NotificationContextValue {
  notifications: AppNotification[];
  unreadCount: number;
}

export const NotificationContext = createContext<NotificationContextValue>({
  notifications: [],
  unreadCount: 0,
});

interface Props {
  children: ReactNode;
}

export function NotificationProvider({ children }: Props) {
  const [notifications, setNotifications] = useState<AppNotification[]>([]);

  const handleNotification = useCallback((notification: AppNotification) => {
    setNotifications((current) => [notification, ...current]);
  }, []);

  useEffect(() => {
    const apiUrl = process.env.NEXT_PUBLIC_API_URL;

    if (!apiUrl) {
      console.error("NEXT_PUBLIC_API_URL is missing");

      return;
    }

    const connection = new HubConnectionBuilder()
      .withUrl(`${apiUrl}/hubs/notifications`, {
        withCredentials: true,
      })
      .withAutomaticReconnect()
      .configureLogging(LogLevel.Warning)
      .build();

    connection.on("notification", handleNotification);

    connection
      .start()
      .then(() => console.log("Notification hub connected"))
      .catch((error) => console.error("Notification hub error", error));

    return () => {
      connection.off("notification", handleNotification);

      connection.stop();
    };
  }, [handleNotification]);

  const unreadCount = notifications.filter((x) => x.readAtUtc === null).length;

  return (
    <NotificationContext.Provider
      value={{
        notifications,
        unreadCount,
      }}
    >
      {children}
    </NotificationContext.Provider>
  );
}
