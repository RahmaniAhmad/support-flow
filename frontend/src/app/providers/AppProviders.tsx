"use client";

import QueryProvider from "./QueryProvider";
import MessageProvider from "./MessageProvider";
import { NotificationProvider } from "@/features/notifications/provider/NotificationProvider";

export default function AppProviders({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <QueryProvider>
      <MessageProvider>
        <NotificationProvider>{children}</NotificationProvider>
      </MessageProvider>
    </QueryProvider>
  );
}
