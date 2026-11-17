"use client";

import DataTable from "@/components/ui/table/DataTable";
import type { SortDirection } from "@/types/common";
import type {
  TicketListItem,
  TicketSortBy,
} from "../types";
import type { TableProps } from "antd";
import type { ColumnsType } from "antd/es/table";

const DEFAULT_SORT_BY: TicketSortBy = "CreatedAtUtc";
const DEFAULT_SORT_DIRECTION: SortDirection = "desc";

const SORT_FIELDS: Record<string, TicketSortBy> = {
  subject: "Subject",
  status: "Status",
  createdAtUtc: "CreatedAtUtc",
};

interface Props {
  data: TicketListItem[];
  loading: boolean;
  columns: ColumnsType<TicketListItem>;
  pagination?: {
    current?: number;
    pageSize?: number;
    total?: number;
  };
  onTableChange(
    page: number,
    pageSize: number,
    sortBy: TicketSortBy,
    sortDirection: SortDirection,
  ): void;
}

export default function TicketTable({
  data,
  loading,
  columns,
  pagination,
  onTableChange,
}: Props) {
  const handleChange: NonNullable<
    TableProps<TicketListItem>["onChange"]
  > = (tablePagination, _filters, sorter) => {
    const activeSorter = Array.isArray(sorter)
      ? sorter.find((item) => item.order)
      : sorter;
    const field = activeSorter?.columnKey ?? activeSorter?.field;
    const fieldName = Array.isArray(field) ? field.join(".") : String(field ?? "");
    const nextSortBy = activeSorter?.order
      ? SORT_FIELDS[fieldName] ?? DEFAULT_SORT_BY
      : DEFAULT_SORT_BY;
    const nextSortDirection =
      activeSorter?.order === "ascend" ? "asc" : DEFAULT_SORT_DIRECTION;

    onTableChange(
      tablePagination.current ?? 1,
      tablePagination.pageSize ?? pagination?.pageSize ?? 10,
      nextSortBy,
      nextSortDirection,
    );
  };

  return (
    <DataTable
      columns={columns}
      dataSource={data}
      loading={loading}
      rowKey="id"
      pagination={{
        current: pagination?.current ?? 1,
        pageSize: pagination?.pageSize ?? 10,
        total: pagination?.total ?? 0,
        showSizeChanger: true,
        pageSizeOptions: [10, 20, 50, 100],
      }}
      onChange={handleChange}
    />
  );
}
