"use client";

import DataTable from "@/components/ui/table/DataTable";
import { TicketListItem } from "../types";
import { ColumnsType } from "antd/es/table";

interface Props {
  data: TicketListItem[];
  loading: boolean;
  columns: ColumnsType<TicketListItem>;
  pagination?: {
    current?: number;
    pageSize?: number;
    total?: number;
  };
  onPaginationChange(page: number, pageSize: number): void;
}

export default function TicketTable({
  data,
  loading,
  columns,
  pagination,
  onPaginationChange,
}: Props) {
  return (
    <DataTable
      columns={columns}
      dataSource={data}
      loading={loading}
      rowKey="id"
      pagination={{
        current: pagination?.current,
        pageSize: pagination?.pageSize,
        total: pagination?.total,
        onChange: onPaginationChange,
      }}
    />
  );
}
