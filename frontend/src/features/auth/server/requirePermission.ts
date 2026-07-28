import { redirect } from "next/navigation";

import { getCurrentUser } from "./getCurrentUser";
import { hasPermission } from "../authorization";
import { Permission } from "../Permissions";

export async function requirePermission(permission: Permission) {
  const currentUser = await getCurrentUser();

  if (!currentUser) {
    redirect("/login");
  }

  if (!hasPermission(currentUser, permission)) {
    redirect("/unauthorized");
  }

  return currentUser;
}
