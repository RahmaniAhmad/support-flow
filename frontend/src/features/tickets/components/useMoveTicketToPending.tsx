"use client";

import Button from "@/components/ui/Button";
import { useMoveTicketToPending } from "../hooks/useMoveTicketToPending";

export default function MoveToPendingAction({
  ticketId,
}: {
  ticketId: string;
}) {
  const mutation = useMoveTicketToPending();

  return (
    <Button type="text" onClick={() => mutation.mutate(ticketId)}>
      Move to pending
    </Button>
  );
}
