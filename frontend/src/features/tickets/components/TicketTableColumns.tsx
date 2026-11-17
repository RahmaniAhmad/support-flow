import { ColumnsType } from "antd/es/table";
import { TicketListItem } from "../types";
import TicketActions from "./TicketActions";
import { CurrentUser } from "@/types/user";
import { Tag } from "antd";
import { priorityColor } from "../utils/priorityColor";
import type { SortDirection } from "@/types/common";
import type { TicketSortBy } from "../types";

export function getTicketColumns(
  currentUser: CurrentUser,
  sortBy: TicketSortBy = "CreatedAtUtc",
  sortDirection: SortDirection = "desc",
): ColumnsType<TicketListItem> {
  const columns: ColumnsType<TicketListItem> = [];
  const activeSortOrder = sortDirection === "asc" ? "ascend" : "descend";

  columns.push({
    title: "Ticket",
    dataIndex: "ticketNumber",
    render: (value: number) => `#${value}`,
  });

  if (currentUser.role === "SuperAdmin") {
    columns.push(
      {
        title: "Company",
        dataIndex: "companyName",
      },
      {
        title: "Created By",
        dataIndex: "createdBy",
      },
    );
  }

  if (currentUser.role === "Admin" || currentUser.role === "Agent") {
    columns.push({
      title: "Created By",
      dataIndex: "createdBy",
    });
  }

  columns.push(
    {
      title: "Subject",
      dataIndex: "subject",
      sorter: true,
      sortOrder: sortBy === "Subject" ? activeSortOrder : null,
    },
    {
      title: "Category",
      dataIndex: "categoryName",
      render: (value: string | undefined) => value ?? "-",
    },
    {
      title: "Status",
      dataIndex: "status",
      sorter: true,
      sortOrder: sortBy === "Status" ? activeSortOrder : null,
    },
    {
      title: "Priority",
      dataIndex: "priority",
      render: (priority: TicketListItem["priority"]) => (
        <Tag color={priorityColor(priority)}>{priority}</Tag>
      ),
    },
    {
      title: "Assignee",
      dataIndex: "assigneeName",
      render: (value) => value ?? "-",
    },
    {
      title: "Created",
      dataIndex: "createdAtUtc",
      render: (value) => new Date(value).toLocaleString(),
      sorter: true,
      sortOrder: sortBy === "CreatedAtUtc" ? activeSortOrder : null,
      responsive: ["md"],
    },
    {
      title: "Actions",
      render: (_, record) => <TicketActions ticket={record} />,
    },
  );

  return columns;
}
