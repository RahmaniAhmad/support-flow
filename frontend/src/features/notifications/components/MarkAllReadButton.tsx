"use client";

import { Button } from "antd";
import { useMarkAllAsRead } from "../hooks/useMarkAllAsRead";

export function MarkAllReadButton() {
  const mutation = useMarkAllAsRead();

  return (
    <Button
      type="link"
      disabled={mutation.isPending}
      onClick={() => mutation.mutate()}
      className="text-sm text-white!"
    >
      {mutation.isPending ? "Marking..." : "Mark all as read"}
    </Button>
  );
}
