"use client";

import { Table } from "antd";

import type { DataTableProps } from "./types";

export default function DataTable<T extends object>({
  columns,
  dataSource,
  loading = false,
  rowKey,
  pagination,
  onChange,
}: DataTableProps<T>) {
  return (
    <Table<T>
      columns={columns}
      dataSource={dataSource}
      loading={loading}
      rowKey={rowKey}
      pagination={pagination}
      onChange={onChange}
      size="medium"
      scroll={{
        x: "max-content",
      }}
    />
  );
}
