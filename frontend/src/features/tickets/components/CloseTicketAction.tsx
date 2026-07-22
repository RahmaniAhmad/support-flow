"use client";

import { Modal, message } from "antd";
import { useCloseTicket } from "../hooks/useCloseTicket";

type Props = {
  ticketId: string;
  onDone?: () => void;
};

export default function CloseTicketAction({ ticketId, onDone }: Props) {
  const { mutate } = useCloseTicket();

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

              onDone?.();
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

  return <span onClick={handleClose}>Close ticket</span>;
}
