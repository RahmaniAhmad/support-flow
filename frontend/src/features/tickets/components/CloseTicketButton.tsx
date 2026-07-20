"use client";

import { Button, Modal, message } from "antd";

import { useCloseTicket } from "../hooks/useCloseTicket";

type Props = {
  ticketId: string;
};

export default function CloseTicketButton({ ticketId }: Props) {
  const { mutate, isPending } = useCloseTicket();

  const handleClose = () => {
    Modal.confirm({
      title: "Close ticket",
      content:
        "Are you sure you want to close this ticket? You cannot add new comments after closing.",

      okText: "Yes, close",
      cancelText: "Cancel",

      okButtonProps: {
        danger: true,
      },

      centered: true,

      onOk: () =>
        new Promise<void>((resolve, reject) => {
          mutate(ticketId, {
            onSuccess: () => {
              message.success("Ticket closed successfully.");
              resolve();
            },

            onError: () => {
              message.error("Failed to close ticket.");
              reject();
            },
          });
        }),
    });
  };

  return (
    <Button danger loading={isPending} onClick={handleClose}>
      Close
    </Button>
  );
}
