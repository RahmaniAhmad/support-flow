"use client";

import QueryProvider from "./QueryProvider";
import MessageProvider from "./MessageProvider";

export default function AppProviders({
  children,
}: {
  children: React.ReactNode;
}) {
  return (
    <QueryProvider>
      <MessageProvider>{children}</MessageProvider>
    </QueryProvider>
  );
}
