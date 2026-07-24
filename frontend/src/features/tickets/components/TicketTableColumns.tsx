import { TicketSummary } from "@/types/ticket";
import { ColumnsType } from "antd/es/table";
import TicketActions from "./TicketActions";

export const ticketColumns: ColumnsType<TicketSummary> = [
  {
    title: "Ticket",
    dataIndex: "ticketNumber",
    key: "ticketNumber",
    render: (value: number) => `#${value}`,
  },
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
];
