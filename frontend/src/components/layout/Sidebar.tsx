"use client";

import Link from "next/link";
import { usePathname } from "next/navigation";

type Props = {
  onClose?: () => void;
};

const menuItems = [
  {
    label: "Dashboard",
    href: "/dashboard",
  },
  {
    label: "Tickets",
    href: "/tickets",
  },
];

export default function Sidebar({ onClose }: Props) {
  const pathname = usePathname();

  return (
    <aside className="flex h-full w-64 flex-col border-r border-gray-300 bg-white">
      <div className="border-b border-gray-300 p-6 text-xl h-16 font-bold">
        VSA Tickets
      </div>

      <nav className="flex flex-col gap-1 p-4">
        {menuItems.map((item) => {
          const isActive = pathname === item.href;

          return (
            <Link
              key={item.href}
              href={item.href}
              onClick={onClose}
              className={`rounded p-3 transition ${
                isActive
                  ? "bg-slate-200 font-semibold text-slate-900"
                  : "text-slate-600 hover:bg-slate-100"
              }`}
            >
              {item.label}
            </Link>
          );
        })}
      </nav>
    </aside>
  );
}
