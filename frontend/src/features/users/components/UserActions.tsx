"use client";

import { Dropdown, Button, Modal } from "antd";
import type { MenuProps } from "antd";
import { UserListItem } from "../types";
import { useChangeUserStatus } from "../hooks/useChangeUserStatus";
import Link from "next/link";
import { EllipsisVertical } from "lucide-react";

interface Props {
  user: UserListItem;
}

export default function UserActions({ user }: Props) {
  const [modal, contextHolder] = Modal.useModal();

  const changeStatusMutation = useChangeUserStatus();

  const handleChangeStatus = () => {
    const isActive = !user.isActive;

    modal.confirm({
      title: isActive ? "Activate user?" : "Deactivate user?",

      content: isActive
        ? "This user will be able to login again."
        : "This user will no longer be able to login.",

      okText: isActive ? "Activate" : "Deactivate",

      onOk: () =>
        changeStatusMutation.mutateAsync({
          userId: user.id,
          isActive,
        }),
    });
  };

  const items: MenuProps["items"] = [
    {
      key: "details",
      label: <Link href={`/users/${user.id}`}>View details</Link>,
    },
    {
      key: "edit",
      label: <Link href={`/users/${user.id}/edit`}>Edit user</Link>,
    },

    {
      key: "change-status",
      label: user.isActive ? "Deactivate" : "Activate",
    },
  ];

  const handleAction: MenuProps["onClick"] = ({ key }) => {
    switch (key) {
      case "change-status":
        handleChangeStatus();
        break;
    }
  };

  return (
    <>
      {contextHolder}
      <Dropdown
        menu={{
          items,
          onClick: handleAction,
        }}
        trigger={["click"]}
      >
        <Button type="text" icon={<EllipsisVertical size={16} />} />
      </Dropdown>
    </>
  );
}
