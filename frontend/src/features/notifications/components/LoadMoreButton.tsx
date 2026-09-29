"use client";

import { Button } from "antd";

interface Props {
  hasMore: boolean;
  loading: boolean;
  onClick: () => void;
}

export function LoadMoreButton({ hasMore, loading, onClick }: Props) {
  if (!hasMore) {
    return null;
  }

  return (
    <div
      className="
        border-t
        p-2
        text-center
      "
    >
      <Button type="link" loading={loading} onClick={onClick}>
        Load more
      </Button>
    </div>
  );
}
