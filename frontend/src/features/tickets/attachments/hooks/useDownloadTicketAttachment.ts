import { useMutation } from "@tanstack/react-query";
import { downloadTicketAttachment } from "../api/attachments";

export function useDownloadTicketAttachment() {
  return useMutation({
    mutationFn: downloadTicketAttachment,
  });
}
