import { AppPermissions, Permission } from "@/features/auth/Permissions";

export type MenuItem = {
  label: string;
  href: string;
  permission?: Permission;
  isActive: (pathname: string) => boolean;
};

const isDetailsRoute =
  (basePath: string) =>
  (pathname: string): boolean =>
    pathname === basePath ||
    new RegExp(`^${basePath}/[^/]+/details$`).test(pathname);

export const MENU_ITEMS: readonly MenuItem[] = [
  {
    label: "Dashboard",
    href: "/dashboard",
    permission: AppPermissions.DashboardView,
    isActive: (pathname) => pathname === "/dashboard",
  },
  {
    label: "Tickets",
    href: "/tickets",
    permission: AppPermissions.TicketsView,
    isActive: isDetailsRoute("/tickets"),
  },
  {
    label: "Users",
    href: "/users",
    permission: AppPermissions.UsersView,
    isActive: (pathname) =>
      pathname === "/users" || pathname.startsWith("/users/"),
  },
];
