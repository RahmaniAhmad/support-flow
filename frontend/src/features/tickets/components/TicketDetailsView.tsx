"use client";

import { Card, Descriptions, Tag, Typography, Spin } from "antd";

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
    <div className="space-y-6">
      <Card>
        <div className="flex justify-between items-start">
          <div>
            <Title level={2}>{ticket.subject}</Title>

            <div>Ticket #{ticket.id}</div>
          </div>

          <Tag color={statusColor(ticket.status)}>{ticket.status}</Tag>
        </div>
      </Card>

      <Card title="Description">
        <Paragraph>{ticket.description}</Paragraph>
      </Card>

      {/* Information */}
      <Card title="Information">
        <Descriptions column={2} bordered>
          <Descriptions.Item label="Created">
            {new Date(ticket.createdAtUtc).toLocaleString()}
          </Descriptions.Item>

          <Descriptions.Item label="Assignee">
            {ticket.assigneeName ?? "-"}
          </Descriptions.Item>

          <Descriptions.Item label="Created By">
            {ticket.createdByName}
          </Descriptions.Item>

          <Descriptions.Item label="Updated">
            {new Date(ticket.updatedAtUtc).toLocaleString()}
          </Descriptions.Item>
        </Descriptions>
      </Card>
    </div>
  );
}
