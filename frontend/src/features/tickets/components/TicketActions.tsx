"use client";

import { Space } from "antd";

import type { TicketStatus, TicketSummary } from "@/types/ticket";

import CloseTicketButton from "./CloseTicketButton";
import ReopenTicketButton from "./ReopenTicketButton";

type Props = {
  ticket: TicketSummary;
};

export default function TicketActions({ ticket }: Props) {
  return (
    <Space>
      {canClose(ticket.status) && <CloseTicketButton ticketId={ticket.id} />}

      {canReopen(ticket.status) && <ReopenTicketButton ticketId={ticket.id} />}
    </Space>
  );
}

function canClose(status: TicketStatus) {
  return status !== "Closed" && status !== "Resolved";
}

function canReopen(status: TicketStatus) {
  return status === "Closed";
}
