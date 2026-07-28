"use client";

import DataTable from "@/components/ui/table/DataTable";
import PageTitle from "@/components/ui/page/PageTitle";

import { useUsers } from "../hooks/useUsers";
import { getUserColumns } from "./UserTableColumns";
import { useCurrentUser } from "@/features/auth/providers/CurrentUserProvider";
import CreateUserButton from "./CreateUserButton";

export default function UserList() {
  const currentUser = useCurrentUser();
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
        <CreateUserButton />
      </div>

      <DataTable
        columns={getUserColumns(currentUser)}
        dataSource={data ?? []}
        loading={isLoading}
        rowKey="id"
      />
    </div>
  );
}
