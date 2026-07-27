"use client";

import { Card, Tag, Typography, Spin } from "antd";

import { useUser } from "../hooks/useUser";

const { Title } = Typography;

type Props = {
  userId: string;
};

export default function UserDetailsView({ userId }: Props) {
  const { data: user, isLoading } = useUser(userId);

  if (isLoading) {
    return <Spin />;
  }

  if (!user) {
    return <Card>User not found</Card>;
  }

  return (
    <div className="flex flex-col gap-6">
      <Card>
        <div className="flex items-start justify-between gap-4">
          <div className="min-w-0">
            <Title level={2} className="mb-2!">
              {user.firstName} {user.lastName}
            </Title>

            <div className="text-sm text-slate-500">{user.email}</div>
          </div>

          <Tag color={user.isActive ? "green" : "red"}>
            {user.isActive ? "Active" : "Inactive"}
          </Tag>
        </div>
      </Card>

      <Card title="Information">
        <div className="grid grid-cols-1 gap-5 md:grid-cols-2">
          <InfoItem label="Role" value={user.role} />

          <InfoItem label="Company" value={user.companyName ?? "-"} />

          <InfoItem label="Email" value={user.email} breakText />

          <InfoItem label="Phone" value={user.phone ?? "-"} />

          <InfoItem
            label="Created"
            value={formatDate(user.createdAtUtc)}
            nowrap
          />

          <InfoItem
            label="Status"
            value={user.isActive ? "Active" : "Inactive"}
          />
        </div>
      </Card>
    </div>
  );
}

function InfoItem({
  label,
  value,
  nowrap = false,
  breakText = false,
}: {
  label: string;
  value: string;
  nowrap?: boolean;
  breakText?: boolean;
}) {
  return (
    <div>
      <div className="mb-1 text-sm text-slate-500">{label}</div>

      <div
        className={[
          "font-medium text-slate-800",
          nowrap ? "whitespace-nowrap" : "",
          breakText ? "break-all" : "",
        ].join(" ")}
      >
        {value}
      </div>
    </div>
  );
}

function formatDate(value: string) {
  return new Date(value).toLocaleString();
}
