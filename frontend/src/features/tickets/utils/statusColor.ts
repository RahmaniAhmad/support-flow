import { TicketStatus } from "@/types/ticket";

export function statusColor(status: TicketStatus) {
  switch (status) {
    case "Open":
      return "green";

    case "Assigned":
      return "blue";

    case "InProgress":
      return "processing";

    case "Resolved":
      return "gold";

    case "Reopened":
      return "orange";

    case "Closed":
      return "default";

    default:
      return "default";
  }
}
