"use client";

import { Card, Tag, Typography, Spin } from "antd";

import { useTicket } from "../hooks/useTicket";
import { statusColor } from "../utils/statusColor";

const { Title, Paragraph } = Typography;

type Props = {
  ticketId: string;
};

export default function TicketDetailsView({ ticketId }: Props) {
  const { data: ticket, isLoading } = useTicket(ticketId);

  if (isLoading) {
    return <Spin />;
  }

  if (!ticket) {
    return <Card>Ticket not found</Card>;
  }

  return (
    <div className="flex flex-col gap-6">
      <Card>
        <div className="flex items-start justify-between gap-4">
          <div className="min-w-0">
            <Title level={2} className="mb-2!">
              {ticket.subject}
            </Title>

            <div className="text-sm text-slate-500">
              Ticket #{ticket.ticketNumber}
            </div>
          </div>

          <Tag color={statusColor(ticket.status)}>{ticket.status}</Tag>
        </div>
      </Card>

      <Card title="Description">
        <Paragraph className="mb-0!">{ticket.description}</Paragraph>
      </Card>

      <Card title="Information">
        <div className="grid grid-cols-1 gap-5 md:grid-cols-2">
          <InfoItem
            label="Created"
            value={formatDate(ticket.createdAtUtc)}
            nowrap
          />

          <InfoItem label="Assignee" value={ticket.assigneeName ?? "-"} />

          <InfoItem label="Created By" value={ticket.createdByName} breakText />

          <InfoItem
            label="Updated"
            value={ticket.updatedAtUtc ? formatDate(ticket.updatedAtUtc) : "-"}
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
