import { TicketPriority } from "@/types/ticket";

export function priorityColor(priority: TicketPriority) {
  switch (priority) {
    case "Critical":
      return "red";
    case "High":
      return "orange";
    case "Normal":
      return "blue";
    case "Low":
      return "default";
  }
}
