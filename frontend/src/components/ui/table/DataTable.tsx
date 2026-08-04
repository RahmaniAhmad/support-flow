"use client";

import { Skeleton, Table } from "antd";
import type { ColumnsType } from "antd/es/table";

import type { DataTableProps } from "./types";

function createSkeletonColumns<T extends object>(
  columns: ColumnsType<T>,
): ColumnsType<T> {
  return columns.map((column, index) => ({
    ...column,
    key:
      column.key ??
      ("dataIndex" in column ? column.dataIndex?.toString() : undefined) ??
      index,

    render: () => <Skeleton.Input active size="small" />,
  }));
}

function createSkeletonData<T extends object>(rows: number): T[] {
  return Array.from(
    { length: rows },
    (_, index) =>
      ({
        key: `skeleton-${index}`,
      }) as T,
  );
}

export default function DataTable<T extends object>({
  columns,
  dataSource,
  loading = false,
  rowKey,
  pagination,
  onChange,
  onRow,
  skeletonRows = 5,
}: DataTableProps<T>) {
  const tableColumns = loading ? createSkeletonColumns(columns) : columns;

  const tableData = loading ? createSkeletonData<T>(skeletonRows) : dataSource;

  return (
    <Table<T>
      columns={tableColumns}
      dataSource={tableData}
      rowKey={loading ? "key" : rowKey}
      pagination={pagination}
      onChange={onChange}
      onRow={onRow}
      size="middle"
      scroll={{
        x: "max-content",
      }}
    />
  );
}
