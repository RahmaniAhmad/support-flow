import { CurrentUser } from "@/types/user";
import { Permission } from "./Permissions";

export function hasPermission(
  user: CurrentUser | null | undefined,
  permission: Permission,
): boolean {
  return !!user?.permissions.includes(permission);
}
