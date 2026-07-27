"use client";

import { Dropdown, Button, App, Modal } from "antd";
import { MoreOutlined } from "@ant-design/icons";
import type { MenuProps } from "antd";
import { UserListItem } from "../types";
import { useChangeUserStatus } from "../hooks/useChangeUserStatus";

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
      key: "edit",
      label: "Edit user",
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

      case "edit":
        // navigate to edit page
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
        <Button type="text" icon={<MoreOutlined />} />
      </Dropdown>
    </>
  );
}
