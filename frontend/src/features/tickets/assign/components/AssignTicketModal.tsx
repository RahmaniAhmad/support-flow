"use client";

import { Drawer, Modal } from "antd";

import { useIsMobile } from "@/hooks/useIsMobile";

import AssignTicketForm from "./AssignTicketForm";

type Props = {
  ticketId: string;
  ticketSubject: string;
  open: boolean;
  onClose: () => void;
};

export default function AddCommentModal({
  ticketId,
  ticketSubject,
  open,
  onClose,
}: Props) {
  const isMobile = useIsMobile();

  const content = <AssignTicketForm ticketId={ticketId} onSuccess={onClose} />;

  if (isMobile) {
    return (
      <Drawer
        title={`Assign user to "${ticketSubject}"`}
        placement="bottom"
        open={open}
        onClose={onClose}
        size="auto"
        className="rounded-t-2xl"
      >
        {content}
      </Drawer>
    );
  }

  return (
    <Modal
      title={`Assign user to "${ticketSubject}"`}
      open={open}
      onCancel={onClose}
      footer={null}
      destroyOnHidden
    >
      {content}
    </Modal>
  );
}
