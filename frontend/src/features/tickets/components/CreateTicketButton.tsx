"use client";

import { Button } from "antd";
import { useRouter } from "next/navigation";

export default function CreateTicketButton() {
  const router = useRouter();

  return (
    <Button type="primary" onClick={() => router.push("/tickets/create")}>
      Create Ticket
    </Button>
  );
}
