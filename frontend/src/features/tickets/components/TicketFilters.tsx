"use client";

import { TicketStatus } from "@/types/ticket";
import { Input, Select } from "antd";
import { useEffect, useState } from "react";
import { TicketView } from "../types";

type Props = {
  search?: string;
  status?: TicketStatus;
  view: TicketView;

  onSearch(value: string): void;
  onStatusChange(value?: TicketStatus): void;
  onViewChange(view: TicketView): void;
};

export default function TicketFilters({
  search = "",
  status,
  view,
  onSearch,
  onStatusChange,
  onViewChange,
}: Props) {
  const [searchValue, setSearchValue] = useState(search);

  useEffect(() => {
    const timer = setTimeout(() => {
      onSearch(searchValue);
    }, 500);

    return () => clearTimeout(timer);
  }, [searchValue, onSearch]);

  return (
    <div
      className="flex
      flex-col
      gap-3
      mb-4
      sm:flex-row"
    >
      <Input.Search
        className="w-full sm:w-80"
        placeholder="Search tickets..."
        allowClear
        value={searchValue}
        onChange={(e) => setSearchValue(e.target.value)}
      />

      <Select<TicketStatus>
        className="min-w-32"
        placeholder="Status"
        allowClear
        value={status}
        onChange={onStatusChange}
        options={[
          {
            label: "Open",
            value: "Open",
          },
          {
            label: "Reopened",
            value: "Reopened",
          },
          {
            label: "Assigned",
            value: "Assigned",
          },
          {
            label: "Resolved",
            value: "Resolved",
          },
          {
            label: "Closed",
            value: "Closed",
          },
        ]}
      />
      <Select<TicketView>
        className="min-w-44"
        value={view}
        onChange={onViewChange}
        options={[
          {
            label: "All",
            value: "All",
          },
          {
            label: "Assigned to Me",
            value: "AssignedToMe",
          },
          {
            label: "Created by Me",
            value: "CreatedByMe",
          },
        ]}
      />
    </div>
  );
}
