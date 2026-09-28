"use client";

import { Badge, Button, Dropdown } from "antd";

import { BellOutlined } from "@ant-design/icons";

import { NotificationDropdown } from "./NotificationDropdown";
import { useNotifications } from "../hooks/useNotificationHub";

export default function NotificationBell() {
  const { unreadCount } = useNotifications();

  return (
    <Dropdown trigger={["click"]} popupRender={() => <NotificationDropdown />}>
      <Badge count={unreadCount} overflowCount={99}>
        <Button type="text" icon={<BellOutlined />} />
      </Badge>
    </Dropdown>
  );
}
