"use client";

import { CurrentUser } from "@/types/user";
import { createContext, useContext } from "react";

const CurrentUserContext = createContext<CurrentUser | null>(null);

export function CurrentUserProvider({
  currentUser,
  children,
}: {
  currentUser: CurrentUser;
  children: React.ReactNode;
}) {
  return (
    <CurrentUserContext.Provider value={currentUser}>
      {children}
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
