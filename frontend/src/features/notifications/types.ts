export enum NotificationType {
  TicketAssigned = 1,
  TicketCommentAdded = 2,
  TicketStatusChanged = 3,
}

export interface AppNotification {
  id: string;
  userId: string;
  companyId: string;
  ticketId: string;
  type: NotificationType;
  title: string;
  message: string;
  createdAtUtc: string;
  readAtUtc: string | null;
}
