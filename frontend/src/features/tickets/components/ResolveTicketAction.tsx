"use client";

import Button from "@/components/ui/Button";
import { useResolveTicket } from "../hooks/useResolveTicket";

export default function ResolveTicketAction({
  ticketId,
}: {
  ticketId: string;
}) {
  const mutation = useResolveTicket();

  return (
    <Button type="text" onClick={() => mutation.mutate(ticketId)}>
      Resolve
    </Button>
  );
}
