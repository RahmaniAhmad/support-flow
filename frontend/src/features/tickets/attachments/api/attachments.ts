import api from "@/lib/api/axios";
import { TicketAttachment } from "../types";

export async function getTicketAttachments(
  ticketId: string,
): Promise<TicketAttachment[]> {
  const { data } = await api.get<TicketAttachment[]>(
    `/api/tickets/${ticketId}/attachments`,
  );

  return data;
}

export async function uploadTicketAttachment({
  ticketId,
  file,
  onProgress,
}: {
  ticketId: string;
  file: File;
  onProgress: (percent: number) => void;
}): Promise<TicketAttachment> {
  const formData = new FormData();
  formData.append("file", file);

  const { data } = await api.post<TicketAttachment>(
    `/api/tickets/${ticketId}/attachments`,
    formData,
    {
      onUploadProgress: (progress) => {
        if (progress.total) {
          onProgress(
            Math.round((progress.loaded / progress.total) * 100),
          );
        }
      },
    },
  );

  return data;
}

export async function downloadTicketAttachment(
  attachmentId: string,
): Promise<Blob> {
  const { data } = await api.get<Blob>(
    `/api/attachments/${attachmentId}/download`,
    { responseType: "blob" },
  );

  return data;
}

export async function deleteTicketAttachment(
  attachmentId: string,
): Promise<void> {
  await api.delete(`/api/attachments/${attachmentId}`);
}
