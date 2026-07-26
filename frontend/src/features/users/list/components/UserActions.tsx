"use client";

import { Dropdown, Button } from "antd";
import { MoreOutlined } from "@ant-design/icons";
import type { MenuProps } from "antd";
import { User } from "@/types/user";

interface Props {
  user: User;
}

export default function UserActions({ user }: Props) {
  const items: MenuProps["items"] = [
    {
      key: "edit",
      label: "Edit user",
    },

    {
      key: "change-role",
      label: "Change role",
    },

    {
      key: "deactivate",
      label: user.isActive ? "Deactivate" : "Activate",
    },
  ];

  const handleAction: MenuProps["onClick"] = ({ key }) => {
    console.log("action:", key, "user:", user.id);
  };

  return (
    <Dropdown
      menu={{
        items,
        onClick: handleAction,
      }}
      trigger={["click"]}
    >
      <Button type="text" icon={<MoreOutlined />} />
    </Dropdown>
  );
}
