export interface TicketAttachment {
  id: string;
  fileName: string;
  contentType: string;
  fileSize: number;
  uploadedByUserId: string;
  createdAtUtc: string;
}
