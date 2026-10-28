"use client";

import { useState } from "react";
import { Alert, Button as AntButton, Input, Tag } from "antd";
import type { ColumnsType } from "antd/es/table";
import DataTable from "@/components/ui/table/DataTable";
import PageTitle from "@/components/ui/page/PageTitle";
import { useCurrentUser } from "@/features/auth/providers/CurrentUserProvider";
import { hasPermission } from "@/features/auth/authorization";
import { AppPermissions } from "@/features/auth/Permissions";
import { useDebounce } from "@/hooks/useDebounce";
import TicketCategoryFormModal from "./TicketCategoryFormModal";
import {
  useSetTicketCategoryStatus,
  useTicketCategories,
} from "../hooks/useTicketCategories";
import { TicketCategory } from "../types";

export default function TicketCategoryList() {
  const currentUser = useCurrentUser();
  const canManage = hasPermission(
    currentUser,
    AppPermissions.TicketCategoriesManage,
  );
  const [page, setPage] = useState(1);
  const [search, setSearch] = useState("");
  const [editingCategory, setEditingCategory] = useState<
    TicketCategory | undefined
  >();
  const [modalOpen, setModalOpen] = useState(false);
  const debouncedSearch = useDebounce(search, 300);
  const { data, isLoading, error } = useTicketCategories({
    page,
    pageSize: 10,
    search: debouncedSearch || undefined,
  });
  const statusMutation = useSetTicketCategoryStatus();

  const columns: ColumnsType<TicketCategory> = [
    {
      title: "Name",
      dataIndex: "name",
    },
    {
      title: "Description",
      dataIndex: "description",
      render: (value: string | undefined) => value || "-",
    },
    {
      title: "Status",
      dataIndex: "isActive",
      render: (isActive: boolean) => (
        <Tag color={isActive ? "green" : "default"}>
          {isActive ? "Active" : "Inactive"}
        </Tag>
      ),
    },
    ...(canManage
      ? [
          {
            title: "Actions",
            key: "actions",
            render: (_: unknown, category: TicketCategory) => (
              <div className="flex gap-2">
                <AntButton
                  type="link"
                  onClick={() => {
                    setEditingCategory(category);
                    setModalOpen(true);
                  }}
                >
                  Edit
                </AntButton>
                <AntButton
                  type="link"
                  loading={statusMutation.isPending}
                  onClick={() =>
                    statusMutation.mutate({
                      id: category.id,
                      isActive: !category.isActive,
                    })
                  }
                >
                  {category.isActive ? "Deactivate" : "Activate"}
                </AntButton>
              </div>
            ),
          },
        ]
      : []),
  ];

  return (
    <section>
      <div className="mb-4 flex flex-col gap-3 sm:flex-row sm:items-center sm:justify-between">
        <PageTitle>Ticket Categories</PageTitle>
        {canManage && (
          <AntButton
            type="primary"
            onClick={() => {
              setEditingCategory(undefined);
              setModalOpen(true);
            }}
          >
            Create category
          </AntButton>
        )}
      </div>

      <Input.Search
        allowClear
        value={search}
        onChange={(event) => {
          setSearch(event.target.value);
          setPage(1);
        }}
        placeholder="Search categories"
        className="mb-4 max-w-sm"
      />

      {error && (
        <Alert
          className="mb-4"
          type="error"
          showIcon
          message="Unable to load ticket categories."
        />
      )}
      {statusMutation.error && (
        <Alert
          className="mb-4"
          type="error"
          showIcon
          message="Unable to change category status."
        />
      )}

      <DataTable
        columns={columns}
        dataSource={data?.items ?? []}
        loading={isLoading}
        rowKey="id"
        pagination={{
          current: page,
          pageSize: 10,
          total: data?.totalCount ?? 0,
          onChange: setPage,
          showSizeChanger: false,
        }}
      />

      <TicketCategoryFormModal
        open={modalOpen}
        category={editingCategory}
        onClose={() => setModalOpen(false)}
      />
    </section>
  );
}
