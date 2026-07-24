"use client";

import { useState } from "react";

import AddCommentModal from "./AddCommentModal";

type Props = {
  ticketId: string;
  ticketSubject: string;
};

export default function AddCommentAction({ ticketId, ticketSubject }: Props) {
  const [open, setOpen] = useState(false);

  return (
    <>
      <span className="w-full text-left" onClick={() => setOpen(true)}>
        Add comment
      </span>

      <AddCommentModal
        ticketId={ticketId}
        ticketSubject={ticketSubject}
        open={open}
        onClose={() => setOpen(false)}
      />
    </>
  );
}
