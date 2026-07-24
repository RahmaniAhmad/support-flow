"use client";

import { useTicket } from "../../hooks/useTicket";
import { useTicketComments } from "../hooks/useTicketComments";
import AddCommentButton from "./AddCommentButton";

type Props = {
  ticketId: string;
};

export default function TicketComments({ ticketId }: Props) {
  const { data: comments, isLoading } = useTicketComments(ticketId);

  const { data: ticket } = useTicket(ticketId);

  if (isLoading) {
    return (
      <div
        className="
        rounded-xl bg-white p-5 shadow
      "
      >
        Loading comments...
      </div>
    );
  }

  return (
    <div
      className="
      rounded-xl bg-white p-5 shadow
    "
    >
      <div className="mb-5 flex items-center justify-between">
        <h3 className="font-semibold">Comments</h3>

        {ticket && (
          <AddCommentButton
            ticketId={ticketId}
            ticketSubject={ticket.subject}
          />
        )}
      </div>
      {comments?.length === 0 && (
        <p className="text-sm text-slate-500">No comments yet.</p>
      )}

      <div className="space-y-4">
        {comments?.map((comment) => (
          <div
            key={comment.id}
            className="
              border-b pb-4 last:border-none
            "
          >
            <div
              className="
              flex justify-between
              text-sm
            "
            >
              <span className="font-medium">{comment.authorName}</span>

              <span className="text-slate-500">
                {new Date(comment.createdAtUtc).toLocaleString()}
              </span>
            </div>

            <p
              className="
              mt-2 text-slate-700
            "
            >
              {comment.content}
            </p>
          </div>
        ))}
      </div>
    </div>
  );
}
