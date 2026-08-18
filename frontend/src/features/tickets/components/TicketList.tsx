"use client";

import { useMemo, useState } from "react";

import CreateTicketButton from "./CreateTicketButton";
import TicketFilters from "./TicketFilters";

import { useTickets } from "../hooks/useTickets";
import { getTicketColumns } from "./TicketTableColumns";
import PageTitle from "@/components/ui/page/PageTitle";
import { useCurrentUser } from "@/features/auth/providers/CurrentUserProvider";
import TicketTable from "./TicketTable";
import { usePathname, useRouter, useSearchParams } from "next/navigation";
import {
  TicketFilter,
  TicketListFilters,
  TicketStatusParam,
  TicketView,
  TicketViewParam,
} from "../types";
import {
  ticketStatusParamMap,
  ticketStatusUrlMap,
  ticketViewParamMap,
  ticketViewUrlMap,
} from "../utils/TicketFilters";

export default function TicketList() {
  const currentUser = useCurrentUser();

  const pathname = usePathname();
  const searchParams = useSearchParams();

  const statusParam = searchParams.get("status");
  const viewParam = searchParams.get("view");

  const initialStatus: TicketFilter | undefined =
    statusParam && statusParam in ticketStatusParamMap
      ? ticketStatusParamMap[statusParam as TicketStatusParam]
      : undefined;

  const initialView: TicketView =
    viewParam && viewParam in ticketViewParamMap
      ? ticketViewParamMap[viewParam as TicketViewParam]
      : "All";

  const [filters, setFilters] = useState<TicketListFilters>({
    page: 1,
    pageSize: 10,
    search: "",
    status: initialStatus,
    view: initialView,
    sortBy: "CreatedAtUtc",
    sortDirection: "desc",
  });

  const { data, isLoading } = useTickets(filters);

  const columns = useMemo(() => getTicketColumns(currentUser), [currentUser]);

  const handleStatusChange = (value?: TicketFilter) => {
    setFilters((prev) => ({
      ...prev,
      status: value,
      page: 1,
    }));

    const params = new URLSearchParams(searchParams.toString());

    if (value) {
      params.set("status", ticketStatusUrlMap[value]);
    } else {
      params.delete("status");
    }

    const query = params.toString();

    window.history.replaceState(
      null,
      "",
      query ? `${pathname}?${query}` : pathname,
    );
  };

  const handleViewChange = (value: TicketView) => {
    setFilters((prev) => ({
      ...prev,
      view: value,
      page: 1,
    }));

    const params = new URLSearchParams(searchParams.toString());

    if (value === "All") {
      params.delete("view");
    } else {
      params.set("view", ticketViewUrlMap[value]);
    }

    const query = params.toString();

    window.history.replaceState(
      null,
      "",
      query ? `${pathname}?${query}` : pathname,
    );
  };

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
        onStatusChange={handleStatusChange}
        onViewChange={handleViewChange}
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
