"use client";

import { Button, Dropdown } from "antd";
import { MoreOutlined } from "@ant-design/icons";
import type { MenuProps } from "antd";

import { TicketListItem } from "../types";

import { useCurrentUser } from "@/features/auth/providers/CurrentUserProvider";
import { Permission } from "@/features/auth/Permissions";
import { hasPermission } from "@/features/auth/authorization";

import { getTicketActions } from "../utils/getTicketActions";

interface TicketActionsProps {
  ticket: TicketListItem;
}

export default function TicketActions({ ticket }: TicketActionsProps) {
  const currentUser = useCurrentUser();

  const can = (permission: Permission) =>
    hasPermission(currentUser, permission);

  const items = getTicketActions({
    ticket,
    can,
  });

  const handleAction: MenuProps["onClick"] = ({ key }) => {
    console.log("ticket action:", key, ticket.id);
  };

  return (
    <Dropdown
      trigger={["click"]}
      menu={{
        items,
        onClick: handleAction,
      }}
    >
      <Button type="text" icon={<MoreOutlined />} />
    </Dropdown>
  );
}
