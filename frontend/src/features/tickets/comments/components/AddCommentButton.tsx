"use client";

import { useState } from "react";

import Button from "@/components/ui/Button";
import AddCommentModal from "./AddCommentModal";

type Props = {
  ticketId: string;
  ticketSubject: string;
  variant?: "button" | "action";
};

export default function AddCommentButton({
  ticketId,
  ticketSubject,
  variant = "button",
}: Props) {
  const [open, setOpen] = useState(false);

  return (
    <>
      {variant === "button" ? (
        <Button onClick={() => setOpen(true)}>Add Comment</Button>
      ) : (
        <span
          className="block w-full cursor-pointer text-left"
          onClick={() => setOpen(true)}
        >
          Add comment
        </span>
      )}

      <AddCommentModal
        ticketId={ticketId}
        ticketSubject={ticketSubject}
        open={open}
        onClose={() => setOpen(false)}
      />
    </>
  );
}
