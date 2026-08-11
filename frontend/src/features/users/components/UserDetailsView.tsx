"use client";

import { Card, Tag, Typography } from "antd";

import UserDetailsActions from "./UserDetailsActions";
import { UserDetails } from "../types";
import { formatDate } from "@/shared/utils/date";

const { Title } = Typography;

type Props = {
  user: UserDetails;
};

export default function UserDetailsView({ user }: Props) {
  const fullName = `${user.firstName} ${user.lastName}`.trim();

  return (
    <div className="flex flex-col gap-6">
      <Card>
        <div className="flex flex-col gap-4 sm:flex-row sm:items-start sm:justify-between">
          <div className="min-w-0">
            <Title level={2} className="mb-1!">
              {fullName || "-"}
            </Title>

            <div className="break-all text-sm text-slate-500">{user.email}</div>
          </div>

          <UserDetailsActions user={user} />
        </div>
      </Card>

      <Card title="Information">
        <div className="grid grid-cols-1 gap-x-8 gap-y-6 md:grid-cols-2">
          <InfoItem label="Role" value={<Tag>{user.role}</Tag>} />

          <InfoItem
            label="Status"
            value={
              <Tag color={user.isActive ? "success" : "default"}>
                {user.isActive ? "Active" : "Inactive"}
              </Tag>
            }
          />

          <InfoItem label="Company" value={user.companyName ?? "-"} />

          <InfoItem label="Email" value={user.email} breakText />

          <InfoItem label="Phone" value={user.phone ?? "-"} />

          <InfoItem
            label="Created"
            value={formatDate(user.createdAtUtc)}
            nowrap
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
  value: React.ReactNode;
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
        ]
          .filter(Boolean)
          .join(" ")}
      >
        {value}
      </div>
    </div>
  );
}
