"use client";

import { useMemo, useState } from "react";

import CreateTicketButton from "./CreateTicketButton";
import TicketFilters from "./TicketFilters";

import { useTickets } from "../hooks/useTickets";
import { getTicketColumns } from "./TicketTableColumns";
import PageTitle from "@/components/ui/page/PageTitle";
import { TicketListFilters } from "../types";
import { useCurrentUser } from "@/features/auth/providers/CurrentUserProvider";
import TicketTable from "./TicketTable";

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

  const columns = useMemo(() => getTicketColumns(currentUser), [currentUser]);

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

      <TicketTable
        columns={columns}
        data={data?.items ?? []}
        loading={isLoading}
        pagination={{
          current: data?.page,
          pageSize: data?.pageSize,
          total: data?.totalCount,
        }}
        onPaginationChange={(page, pageSize) =>
          setFilters((prev) => ({
            ...prev,
            page,
            pageSize,
          }))
        }
      />
    </div>
  );
}
