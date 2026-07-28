import { AppPermissions } from "../Permissions";

export function getHomeRoute(permissions: string[]) {
  if (permissions.includes(AppPermissions.DashboardView)) {
    return "/dashboard";
  }

  if (permissions.includes(AppPermissions.TicketsView)) {
    return "/tickets";
  }

  return "/unauthorized";
}
