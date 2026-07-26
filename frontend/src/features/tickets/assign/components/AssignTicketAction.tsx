"use client";

import { useState } from "react";

import AssignTicketModal from "./AssignTicketModal";

type Props = {
  ticketId: string;
  ticketSubject: string;
};

export default function AssignTicketAction({ ticketId, ticketSubject }: Props) {
  const [open, setOpen] = useState(false);

  return (
    <>
      <span className="w-full text-left" onClick={() => setOpen(true)}>
        Assign ticket
      </span>

      <AssignTicketModal
        ticketId={ticketId}
        ticketSubject={ticketSubject}
        open={open}
        onClose={() => setOpen(false)}
      />
    </>
  );
}
