export type TicketStatus = "Open" | "Assigned" | "Resolved" | "Closed";

export interface TicketSummary {
  id: string;
  title: string;
  status: TicketStatus;
  assigneeName?: string;
  createdAt: string;
}

export interface TicketDetails {
  id: string;
  title: string;
  description: string;
  status: TicketStatus;
  assigneeName?: string;
  createdAt: string;
  comments: TicketComment[];
}

export interface TicketComment {
  id: string;
  text: string;
  authorName: string;
  createdAt: string;
}
