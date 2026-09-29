"use client";

import { Dropdown } from "antd";
import { NotificationButton } from "./NotificationButton";
import { NotificationPanel } from "./NotificationPanel";

export function NotificationCenter() {
  return (
    <Dropdown
      trigger={["click"]}
      placement="bottom"
      popupRender={() => <NotificationPanel />}
    >
      <span>
        <NotificationButton />
      </span>
    </Dropdown>
  );
}
