"use client";

import { Input, Select } from "antd";
import { useEffect, useState } from "react";

import type { TicketStatus } from "@/types/ticket";

type Props = {
  search?: string;
  status?: TicketStatus;

  onSearch(value: string): void;
  onStatusChange(value?: TicketStatus): void;
};

export default function TicketFilters({
  search = "",
  status,
  onSearch,
  onStatusChange,
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
    </div>
  );
}
