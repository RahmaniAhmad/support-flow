import { ColumnsType } from "antd/es/table";
import UserActions from "./UserActions";
import { UserListItem } from "../types";

export const userColumns: ColumnsType<UserListItem> = [
  {
    title: "Name",
    key: "name",
    render: (_, user) => `${user.firstName} ${user.lastName}`,
  },

  {
    title: "Email",
    dataIndex: "email",
    sorter: true,
  },

  {
    title: "Role",
    dataIndex: "role",
    sorter: true,
  },

  {
    title: "Phone",
    dataIndex: "phone",
    render: (value) => value ?? "-",
  },

  {
    title: "Status",
    dataIndex: "isActive",
    render: (value: boolean) => (value ? "Active" : "Inactive"),
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
    render: (_, record) => <UserActions user={record} />,
  },
];
