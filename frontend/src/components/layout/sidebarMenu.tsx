import {
  BookOpen,
  Headset,
  LayoutDashboard,
  LucideIcon,
  Search,
  Users,
} from "lucide-react";

import { AppPermissions, Permission } from "@/features/auth/Permissions";

export type MenuItem = {
  label: string;
  href: string;
  icon: LucideIcon;
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
    icon: LayoutDashboard,
    permission: AppPermissions.DashboardView,
    isActive: (pathname) => pathname === "/dashboard",
  },
  {
    label: "Tickets",
    href: "/tickets",
    icon: Headset,
    permission: AppPermissions.TicketsView,
    isActive: isDetailsRoute("/tickets"),
  },
  {
    label: "Users",
    href: "/users",
    icon: Users,
    permission: AppPermissions.UsersView,
    isActive: (pathname) =>
      pathname === "/users" || pathname.startsWith("/users/"),
  },
  {
    label: "Knowledge Articles",
    href: "/knowledge-articles",
    icon: BookOpen,
    permission: AppPermissions.KnowledgeArticlesView,
    isActive: (pathname) =>
      pathname === "/knowledge-articles" ||
      pathname.startsWith("/knowledge-articles/"),
  },
  {
    label: "AI Search",
    href: "/ai/search",
    icon: Search,
    permission: AppPermissions.AiSemanticSearch,
    isActive: (pathname) => pathname === "/ai/search",
  },
];
