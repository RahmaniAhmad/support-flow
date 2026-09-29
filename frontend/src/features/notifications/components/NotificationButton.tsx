"use client";

import { Badge, Button } from "antd";
import { BellOutlined } from "@ant-design/icons";

import { useUnreadCount } from "../hooks/useUnreadCount";

export function NotificationButton() {
  const { data } = useUnreadCount();

  return (
    <Badge count={data?.count} overflowCount={99}>
      <Button type="text" icon={<BellOutlined />} aria-label="Notifications" />
    </Badge>
  );
}
