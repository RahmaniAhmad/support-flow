"use client";

import { Button, Modal, message } from "antd";

import { useReopenTicket } from "../hooks/useReopenTicket";

type Props = {
  ticketId: string;
};

export default function ReopenTicketButton({ ticketId }: Props) {
  const { mutate, isPending } = useReopenTicket();

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

  return (
    <Button loading={isPending} onClick={handleReopen}>
      Reopen
    </Button>
  );
}
