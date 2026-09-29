"use client";

import { HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import { ReactNode, useEffect } from "react";
import { useQueryClient } from "@tanstack/react-query";

import {
  AppNotification,
  NotificationInfiniteData,
  NotificationType,
  UnreadCountResponse,
} from "../types";
import { queryKeys } from "@/lib/react-query/queryKeys";

interface Props {
  children: ReactNode;
}

export function NotificationProvider({ children }: Props) {
  const queryClient = useQueryClient();

  useEffect(() => {
    let mounted = true;

    const apiUrl = process.env.NEXT_PUBLIC_API_URL;

    if (!apiUrl) {
      console.error("NEXT_PUBLIC_API_URL missing");
      return;
    }

    const connection = new HubConnectionBuilder()
      .withUrl(`${apiUrl}/hubs/notifications`, {
        withCredentials: true,
      })
      .withAutomaticReconnect()
      .configureLogging(LogLevel.Warning)
      .build();

    const handleNotification = (notification: AppNotification) => {
      if (!mounted) {
        return;
      }

      // Update infinite notifications cache
      queryClient.setQueryData<NotificationInfiniteData>(
        queryKeys.notifications.list(),
        (current) => {
          if (!current) {
            return current;
          }

          return {
            ...current,

            pages: current.pages.map((page, index) => {
              if (index !== 0) {
                return page;
              }

              return {
                ...page,

                items: [notification, ...page.items],

                totalCount: page.totalCount + 1,
              };
            }),
          };
        },
      );

      // Update unread count
      queryClient.setQueryData<UnreadCountResponse>(
        queryKeys.notifications.unreadCount(),
        (current) => ({
          count: (current?.count ?? 0) + 1,
        }),
      );

      // Refresh ticket data
      if (
        notification.ticketId &&
        notification.type === NotificationType.TicketStatusChanged
      ) {
        queryClient.invalidateQueries({
          queryKey: queryKeys.tickets.detail(notification.ticketId),
        });

        queryClient.invalidateQueries({
          queryKey: queryKeys.tickets.lists(),
        });
      }

      if (
        notification.ticketId &&
        notification.type === NotificationType.TicketCommentAdded
      ) {
        queryClient.invalidateQueries({
          queryKey: queryKeys.ticketComments.list(notification.ticketId),
        });
      }
    };

    connection.on("notification", handleNotification);

    connection
      .start()
      .then(() => {
        if (mounted) {
          console.log("Notification hub connected");
        }
      })
      .catch((error) => {
        if (mounted) {
          console.error("Notification hub error", error);
        }
      });

    return () => {
      mounted = false;

      connection.off("notification", handleNotification);

      connection.stop().catch(() => {});
    };
  }, [queryClient]);

  return <>{children}</>;
}
