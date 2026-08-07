"use client";

import { Modal, message } from "antd";
import { useStartProgressTicket } from "../hooks/useStartProgressTicket";

type Props = {
  ticketId: string;
  onDone?: () => void;
};

export default function StartProgressAction({ ticketId, onDone }: Props) {
  const { mutate } = useStartProgressTicket();

  const handleStartProgress = () => {
    Modal.confirm({
      title: "Start progress",
      content: "Are you sure you want to start working on this ticket?",

      okText: "Start",
      cancelText: "Cancel",

      centered: true,

      onOk: () =>
        new Promise<void>((resolve, reject) => {
          mutate(ticketId, {
            onSuccess: () => {
              message.success("Ticket moved to In Progress successfully.");

              onDone?.();
              resolve();
            },

            onError: () => {
              message.error("Failed to start ticket progress.");

              reject();
            },
          });
        }),
    });
  };

  return (
    <button
      className="cursor-pointer disabled:cursor-not-allowed"
      type="button"
      onClick={handleStartProgress}
    >
      Start progress
    </button>
  );
}
