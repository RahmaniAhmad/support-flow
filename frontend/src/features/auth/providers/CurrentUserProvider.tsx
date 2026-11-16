"use client";

import { CurrentUser } from "@/types/user";
import { createContext, useContext, useEffect, useState } from "react";
import { QueryClientProvider } from "@tanstack/react-query";
import { createQueryClient } from "@/lib/react-query/queryClient";
import { NotificationProvider } from "@/features/notifications/provider/NotificationProvider";

const CurrentUserContext = createContext<CurrentUser | null>(null);

function UserScopedProviders({ children }: { children: React.ReactNode }) {
  const [queryClient] = useState(createQueryClient);

  useEffect(
    () => () => {
      queryClient.clear();
    },
    [queryClient],
  );

  return (
    <QueryClientProvider client={queryClient}>
      <NotificationProvider>{children}</NotificationProvider>
    </QueryClientProvider>
  );
}

export function CurrentUserProvider({
  currentUser,
  children,
}: {
  currentUser: CurrentUser;
  children: React.ReactNode;
}) {
  return (
    <CurrentUserContext.Provider value={currentUser}>
      <UserScopedProviders
        key={`${currentUser.id}:${currentUser.companyId ?? ""}:${currentUser.role}`}
      >
        {children}
      </UserScopedProviders>
    </CurrentUserContext.Provider>
  );
}

export function useCurrentUser(): CurrentUser {
  const user = useContext(CurrentUserContext);

  if (!user) {
    throw new Error("useCurrentUser must be used within a CurrentUserProvider");
  }

  return user;
}
