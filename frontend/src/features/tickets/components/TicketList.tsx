"use client";

import { useState } from "react";

import CreateTicketButton from "./CreateTicketButton";
import TicketFilters from "./TicketFilters";

import { useTickets } from "../hooks/useTickets";
import { getTicketColumns } from "./TicketTableColumns";
import DataTable from "@/components/ui/table/DataTable";
import PageTitle from "@/components/ui/page/PageTitle";
import { TicketListFilters } from "../types";
import { useCurrentUser } from "@/features/auth/providers/CurrentUserProvider";

export default function TicketList() {
  const currentUser = useCurrentUser();

  const [filters, setFilters] = useState<TicketListFilters>({
    page: 1,
    pageSize: 10,
    search: "",
    status: undefined,
    view: "All",
    sortBy: "CreatedAtUtc",
    sortDirection: "desc",
  });

  const { data, isLoading } = useTickets(filters);

  return (
    <div>
      <div
        className="flex
    flex-col
    gap-3
    mb-4
    sm:flex-row
    sm:justify-between
    sm:items-center"
      >
        <PageTitle>Tickets</PageTitle>
        <CreateTicketButton />
      </div>

      <TicketFilters
        search={filters.search}
        status={filters.status}
        view={filters.view}
        onSearch={(value) =>
          setFilters({
            ...filters,
            search: value,
            page: 1,
          })
        }
        onStatusChange={(value) =>
          setFilters({
            ...filters,
            status: value,
            page: 1,
          })
        }
        onViewChange={(view) =>
          setFilters((prev) => ({
            ...prev,
            view,
            page: 1,
          }))
        }
      />

      <DataTable
        columns={getTicketColumns(currentUser)}
        dataSource={data?.items ?? []}
        loading={isLoading}
        rowKey="id"
        pagination={{
          current: data?.page,
          pageSize: data?.pageSize,
          total: data?.totalCount,

          onChange(page, pageSize) {
            setFilters({
              ...filters,
              page,
              pageSize,
            });
          },
        }}
        onChange={(pagination, filters, sorter) => {
          const s = Array.isArray(sorter) ? sorter[0] : sorter;

          setFilters((prev) => ({
            ...prev,
            page: pagination.current ?? 1,
            pageSize: pagination.pageSize ?? 10,
            sortBy: s.field?.toString(),
            descending: s.order !== "ascend",
          }));
        }}
      />
    </div>
  );
}
