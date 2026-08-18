"use client";

import { Skeleton } from "antd";
interface Props {
  length?: number;
  rows?: number;
}
export function CardSkeleton({ length = 1, rows = 1 }: Props) {
  return (
    <div className="grid gap-6 md:grid-cols-2 xl:grid-cols-4">
      {Array.from({ length: length }).map((_, index) => (
        <div
          key={index}
          className="rounded-xl border border-gray-300 bg-white p-6"
        >
          <Skeleton active paragraph={{ rows: rows }} />
        </div>
      ))}
    </div>
  );
}
