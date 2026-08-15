"use client";

import { useRouter } from "next/navigation";
import { Avatar, Dropdown } from "antd";
import Button from "../ui/Button";
import { useCurrentUser } from "@/features/auth/providers/CurrentUserProvider";
import { ChevronDown, LogOut, Menu, UserRound } from "lucide-react";
import { useLogout } from "@/features/auth/hooks/useLogout";

type Props = {
  onMenuClick: () => void;
};

export default function Navbar({ onMenuClick }: Props) {
  const router = useRouter();
  const currentUser = useCurrentUser();
  const logoutMutation = useLogout();

  const handleLogout = async () => {
    await logoutMutation.mutateAsync();

    router.replace("/login");
  };

  const menuItems = [
    {
      key: "email",
      disabled: true,
      label: (
        <div className="py-1">
          <div className="text-xs text-gray-500">Signed in as</div>
          <div className="max-w-56 truncate font-medium">
            {currentUser?.email ?? "Unknown user"}
          </div>
        </div>
      ),
    },
    {
      type: "divider" as const,
    },
    {
      key: "profile",
      icon: <UserRound size={18} />,
      label: "Profile",
      onClick: () => {
        router.push("/profile");
      },
    },
    {
      type: "divider" as const,
    },
    {
      key: "logout",
      icon: <LogOut size={18} />,
      label: "Logout",
      danger: true,
      onClick: handleLogout,
    },
  ];

  return (
    <header className="flex h-16 items-center justify-between border-b border-gray-300 bg-white px-4 md:px-6">
      <div className="flex items-center gap-3">
        <button
          onClick={onMenuClick}
          className="rounded p-2 transition-colors hover:bg-gray-100 md:hidden"
        >
          <Menu size={18} />
        </button>

        <h1 className="hidden text-lg font-semibold sm:block">
          Ticket Management System
        </h1>

        <h1 className="text-lg font-semibold sm:hidden">TMS</h1>
      </div>

      <Dropdown
        menu={{ items: menuItems }}
        trigger={["click"]}
        placement="bottomRight"
      >
        <Button type="text" className="flex items-center gap-2">
          <Avatar size="small" icon={<UserRound />} />
          <ChevronDown size={18} className="text-gray-500" />
        </Button>
      </Dropdown>
    </header>
  );
}
