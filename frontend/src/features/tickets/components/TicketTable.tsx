"use client";

import DataTable from "@/components/ui/table/DataTable";
import { TicketListItem } from "../types";
import { getTicketColumns } from "./TicketTableColumns";
import { CurrentUser } from "@/types/user";

interface Props {
  data: TicketListItem[];
  loading: boolean;
  pagination?: {
    current?: number;
    pageSize?: number;
    total?: number;
  };
  onPaginationChange(page: number, pageSize: number): void;
  currentUser: CurrentUser;
}

export default function TicketTable({
  data,
  loading,
  pagination,
  onPaginationChange,
  currentUser,
}: Props) {
  return (
    <DataTable
      columns={getTicketColumns(currentUser)}
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
