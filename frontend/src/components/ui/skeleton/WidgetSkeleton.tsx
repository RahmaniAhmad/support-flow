"use client";

import { Skeleton } from "antd";

interface Props {
  rows?: number;
}
export function WidgetSkeleton({ rows = 8 }: Props) {
  return (
    <div className="rounded-xl border border-gray-300 bg-white p-6">
      <Skeleton active title={{ width: "25%" }} paragraph={false} />

      <div className="mt-6 h-87">
        <Skeleton active paragraph={{ rows: rows }} />
      </div>
    </div>
  );
}
