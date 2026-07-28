import { AppPermissions } from "@/features/auth/Permissions";
import { requirePermission } from "@/features/auth/server/requirePermission";
import UserList from "@/features/users/components/UserList";

export default async function UsersPage() {
  await requirePermission(AppPermissions.UsersView);
  return <UserList />;
}
