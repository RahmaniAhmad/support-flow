"use client";

import {
  Alert,
  Button,
  Card,
  Empty,
  Popconfirm,
  Progress,
  Spin,
  Typography,
  Upload,
  message,
} from "antd";
import {
  Download,
  Paperclip,
  Trash2,
  Upload as UploadIcon,
} from "lucide-react";
import { useState } from "react";
import { hasPermission } from "@/features/auth/authorization";
import { AppPermissions } from "@/features/auth/Permissions";
import { useCurrentUser } from "@/features/auth/providers/CurrentUserProvider";
import { useDeleteTicketAttachment } from "../hooks/useDeleteTicketAttachment";
import { useDownloadTicketAttachment } from "../hooks/useDownloadTicketAttachment";
import { useTicketAttachments } from "../hooks/useTicketAttachments";
import { useUploadTicketAttachment } from "../hooks/useUploadTicketAttachment";
import { TicketAttachment } from "../types";
import {
  ACCEPTED_ATTACHMENT_EXTENSIONS,
  MAX_ATTACHMENT_FILE_SIZE,
  MAX_TICKET_ATTACHMENTS,
} from "../constants";

type Props = {
  ticketId: string;
};

export default function TicketAttachments({ ticketId }: Props) {
  const currentUser = useCurrentUser();
  const attachmentsQuery = useTicketAttachments(ticketId);
  const upload = useUploadTicketAttachment();
  const remove = useDeleteTicketAttachment(ticketId);
  const download = useDownloadTicketAttachment();
  const [uploadProgress, setUploadProgress] = useState<number | null>(null);
  const [clientError, setClientError] = useState<string | null>(null);
  const [messageApi, messageContext] = message.useMessage();

  const attachments = attachmentsQuery.data ?? [];
  const canUpload = hasPermission(currentUser, AppPermissions.TicketsCreate);
  const canManageAttachments = hasPermission(
    currentUser,
    AppPermissions.TicketsManageAttachments,
  );

  function beforeUpload(file: File) {
    setClientError(null);
    const extension = file.name.slice(file.name.lastIndexOf(".")).toLowerCase();

    if (file.size === 0) {
      setClientError("Empty files cannot be uploaded.");
      return Upload.LIST_IGNORE;
    }

    if (file.size > MAX_ATTACHMENT_FILE_SIZE) {
      setClientError("The file exceeds the 10 MB limit.");
      return Upload.LIST_IGNORE;
    }

    if (!ACCEPTED_ATTACHMENT_EXTENSIONS.includes(extension)) {
      setClientError("This file extension is not allowed.");
      return Upload.LIST_IGNORE;
    }

    setUploadProgress(0);
    upload.mutate(
      {
        ticketId,
        file,
        onProgress: setUploadProgress,
      },
      {
        onSuccess: () => {
          setUploadProgress(null);
          messageApi.success("Attachment uploaded.");
        },
        onError: () => setUploadProgress(null),
      },
    );

    return Upload.LIST_IGNORE;
  }

  function handleDownload(attachment: TicketAttachment) {
    download.mutate(attachment.id, {
      onSuccess: (blob) => {
        const url = URL.createObjectURL(blob);
        const link = document.createElement("a");
        link.href = url;
        link.download = attachment.fileName;
        link.click();
        URL.revokeObjectURL(url);
      },
    });
  }

  function canDelete(attachment: TicketAttachment) {
    return (
      attachment.uploadedByUserId === currentUser.id || canManageAttachments
    );
  }

  const hasError =
    attachmentsQuery.isError ||
    upload.isError ||
    download.isError ||
    remove.isError;

  return (
    <div>
      {messageContext}
      <Card
        title={
          <span className="flex items-center gap-2">
            <Paperclip size={16} />
            Attachments
          </span>
        }
        extra={
          <Typography.Text type="secondary">
            {attachments.length}/{MAX_TICKET_ATTACHMENTS}
          </Typography.Text>
        }
      >
        {(clientError || hasError) && (
          <Alert
            className="mb-4"
            type="error"
            showIcon
            title={
              clientError ??
              "Unable to complete the attachment operation. Please try again."
            }
            onClose={() => setClientError(null)}
            closable={!!clientError}
          />
        )}

        {canUpload && attachments.length < MAX_TICKET_ATTACHMENTS && (
          <Upload.Dragger
            accept={ACCEPTED_ATTACHMENT_EXTENSIONS.join(",")}
            beforeUpload={beforeUpload}
            disabled={upload.isPending}
            multiple={false}
            showUploadList={false}
          >
            <div className="flex flex-col items-center gap-2 py-2">
              <UploadIcon size={22} />
              <Typography.Text>
                Click or drag a file here to upload
              </Typography.Text>
              <Typography.Text type="secondary">
                PDF, images, text, Word, Excel, or CSV (max 10 MB)
              </Typography.Text>
            </div>
          </Upload.Dragger>
        )}

        {uploadProgress !== null && (
          <Progress className="mt-3" percent={uploadProgress} status="active" />
        )}

        {attachmentsQuery.isLoading ? (
          <div className="flex justify-center py-6">
            <Spin />
          </div>
        ) : attachments.length === 0 ? (
          <Empty
            className="my-6"
            description="No attachments yet."
            image={Empty.PRESENTED_IMAGE_SIMPLE}
          />
        ) : (
          <div className="mt-3 divide-y" role="list">
            {attachments.map((attachment) => (
              <div
                key={attachment.id}
                className="flex items-center justify-between gap-4 py-3"
                role="listitem"
              >
                <div className="min-w-0">
                  <Typography.Text className="break-all">
                    {attachment.fileName}
                  </Typography.Text>
                  <div>
                    <Typography.Text type="secondary">
                      {formatFileSize(attachment.fileSize)}
                    </Typography.Text>
                  </div>
                </div>
                <div className="flex shrink-0 gap-2">
                  <Button
                    aria-label={`Download ${attachment.fileName}`}
                    icon={<Download size={16} />}
                    loading={
                      download.isPending && download.variables === attachment.id
                    }
                    onClick={() => handleDownload(attachment)}
                  />
                  {canDelete(attachment) && (
                    <Popconfirm
                      title="Delete this attachment?"
                      onConfirm={() => remove.mutate(attachment.id)}
                      okButtonProps={{ danger: true }}
                    >
                      <Button
                        danger
                        aria-label={`Delete ${attachment.fileName}`}
                        icon={<Trash2 size={16} />}
                        loading={
                          remove.isPending && remove.variables === attachment.id
                        }
                      />
                    </Popconfirm>
                  )}
                </div>
              </div>
            ))}
          </div>
        )}
      </Card>
    </div>
  );
}

function formatFileSize(fileSize: number) {
  if (fileSize < 1024) {
    return `${fileSize} B`;
  }

  if (fileSize < 1024 * 1024) {
    return `${(fileSize / 1024).toFixed(1)} KB`;
  }

  return `${(fileSize / (1024 * 1024)).toFixed(1)} MB`;
}
