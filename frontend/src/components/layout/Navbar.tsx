"use client";

import { useRouter } from "next/navigation";
import { Avatar, Dropdown } from "antd";
import {
  DownOutlined,
  LogoutOutlined,
  MenuOutlined,
  UserOutlined,
} from "@ant-design/icons";

import Button from "../ui/Button";
import { logout } from "@/features/auth/services/auth.service";
import { useUser } from "@/features/auth/providers/UserProvider";

type Props = {
  onMenuClick: () => void;
};

export default function Navbar({ onMenuClick }: Props) {
  const router = useRouter();
  const user = useUser();

  const handleLogout = async () => {
    await logout();
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
            {user?.email ?? "Unknown user"}
          </div>
        </div>
      ),
    },
    {
      type: "divider" as const,
    },
    {
      key: "logout",
      icon: <LogoutOutlined />,
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
          <MenuOutlined />
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
          <Avatar size="small" icon={<UserOutlined />} />
          <DownOutlined className="text-xs" />
        </Button>
      </Dropdown>
    </header>
  );
}
