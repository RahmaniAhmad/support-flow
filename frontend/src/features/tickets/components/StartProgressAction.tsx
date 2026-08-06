"use client";

import Button from "@/components/ui/Button";
import { useStartProgressTicket } from "../hooks/useStartProgressTicket";

export default function StartProgressAction({
  ticketId,
}: {
  ticketId: string;
}) {
  const mutation = useStartProgressTicket();

  return (
    <Button type="text" onClick={() => mutation.mutate(ticketId)}>
      Start progress
    </Button>
  );
}
