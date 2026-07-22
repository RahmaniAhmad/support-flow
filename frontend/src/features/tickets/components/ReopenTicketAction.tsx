"use client";

import { Modal, message } from "antd";
import { useReopenTicket } from "../hooks/useReopenTicket";

type Props = {
  ticketId: string;
  onDone?: () => void;
};

export default function ReopenTicketAction({ ticketId, onDone }: Props) {
  const { mutate } = useReopenTicket();

  const handleReopen = () => {
    Modal.confirm({
      title: "Reopen ticket",
      content: "Are you sure you want to reopen this ticket?",

      okText: "Yes, reopen",
      cancelText: "Cancel",

      centered: true,

      onOk: () =>
        new Promise<void>((resolve, reject) => {
          mutate(ticketId, {
            onSuccess: () => {
              message.success("Ticket reopened successfully.");

              onDone?.();
              resolve();
            },

            onError: () => {
              message.error("Failed to reopen ticket.");

              reject();
            },
          });
        }),
    });
  };

  return <span onClick={handleReopen}>Reopen ticket</span>;
}
