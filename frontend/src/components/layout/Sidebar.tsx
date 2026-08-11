"use client";

import { useCurrentUser } from "@/features/auth/providers/CurrentUserProvider";
import { hasPermission } from "@/features/auth/authorization";
import Link from "next/link";
import { usePathname } from "next/navigation";
import { MENU_ITEMS } from "./sidebarMenu";

type Props = {
  onClose?: () => void;
};

export default function Sidebar({ onClose }: Props) {
  const pathname = usePathname();

  const currentUser = useCurrentUser();

  const visibleItems = MENU_ITEMS.filter(
    (item) => !item.permission || hasPermission(currentUser, item.permission),
  );

  return (
    <aside className="flex h-full w-64 flex-col border-r border-gray-300 bg-white">
      <div className="border-b border-gray-300 p-6 text-xl h-16 font-bold">
        VSA Tickets
      </div>

      <nav className="flex flex-col gap-1 p-4">
        {visibleItems.map((item) => {
          const isActive = item.isActive?.(pathname) ?? false;
          const Icon = item.icon;

          return (
            <Link
              key={item.href}
              href={item.href}
              onClick={onClose}
              className={`flex items-center gap-3 rounded-lg px-3 py-2.5 transition-colors ${
                isActive
                  ? "bg-slate-200 font-semibold text-slate-900"
                  : "text-slate-600 hover:bg-slate-100"
              }`}
            >
              <Icon size={18} strokeWidth={1.8} />

              <span>{item.label}</span>
            </Link>
          );
        })}
      </nav>
    </aside>
  );
}
