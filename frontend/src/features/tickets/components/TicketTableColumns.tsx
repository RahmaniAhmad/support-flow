import { ColumnsType } from "antd/es/table";
import { TicketListItem } from "../types";
import TicketActions from "./TicketActions";
import { CurrentUser } from "@/types/user";

export function getTicketColumns(
  currentUser: CurrentUser,
): ColumnsType<TicketListItem> {
  const columns: ColumnsType<TicketListItem> = [];

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
        dataIndex: "createdName",
      },
    );
  }

  if (currentUser.role === "Admin" || currentUser.role === "Agent") {
    columns.push({
      title: "Created By",
      dataIndex: "createdName",
    });
  }

  columns.push(
    {
      title: "Subject",
      dataIndex: "subject",
      sorter: true,
    },
    {
      title: "Status",
      dataIndex: "status",
      sorter: true,
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
      responsive: ["md"],
    },
    {
      title: "Actions",
      render: (_, record) => <TicketActions ticket={record} />,
    },
  );

  return columns;
}
