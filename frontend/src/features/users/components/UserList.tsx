"use client";

import DataTable from "@/components/ui/table/DataTable";
import PageTitle from "@/components/ui/page/PageTitle";

import { useUsers } from "../hooks/useUsers";
import { userColumns } from "./UserTableColumns";

export default function UserList() {
  const { data, isLoading } = useUsers();

  return (
    <div>
      <div
        className="
        flex
        flex-col
        gap-3
        mb-4
        sm:flex-row
        sm:justify-between
        sm:items-center"
      >
        <PageTitle>Users</PageTitle>
      </div>

      <DataTable
        columns={userColumns}
        dataSource={data ?? []}
        loading={isLoading}
        rowKey="id"
      />
    </div>
  );
}
