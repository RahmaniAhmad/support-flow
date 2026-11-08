"use client";

import { useRouter } from "next/navigation";
import { zodResolver } from "@hookform/resolvers/zod";
import { useForm } from "react-hook-form";

import Button from "@/components/ui/Button";
import FormInput from "@/components/form/FormInput";
import { useCreateTicket } from "../hooks/useCreateTicket";
import {
  createTicketSchema,
  CreateTicketFormData,
} from "../schemas/create-ticket.schema";
import FormTextArea from "@/components/form/FormTextArea";
import { Alert, Button as AntButton, Typography, Upload } from "antd";
import { Paperclip, Trash2 } from "lucide-react";
import { useRef, useState } from "react";
import { useMessage } from "@/app/providers/MessageProvider";
import FormCard from "@/components/form/FormCard";
import FormLabel from "@/components/form/FormLabel";
import { TICKET_VALIDATION } from "../constants/ticket-validation";
import FormError from "@/components/form/FormError";
import FormSelect from "@/components/form/FormSelect";
import { useActiveTicketCategories } from "@/features/ticket-categories/hooks/useTicketCategories";
import {
  ACCEPTED_ATTACHMENT_EXTENSIONS,
  MAX_ATTACHMENT_FILE_SIZE,
  MAX_TICKET_ATTACHMENTS,
} from "../attachments/constants";

export default function CreateTicketForm() {
  const router = useRouter();

  const message = useMessage();
  const selectedFilesRef = useRef<File[]>([]);
  const [selectedFiles, setSelectedFiles] = useState<File[]>([]);
  const [attachmentError, setAttachmentError] = useState<string | null>(null);

  const mutation = useCreateTicket();
  const {
    data: categories = [],
    isLoading: isLoadingCategories,
    error: categoriesError,
  } = useActiveTicketCategories();

  const { control, handleSubmit } = useForm<CreateTicketFormData>({
    resolver: zodResolver(createTicketSchema),
    defaultValues: {
      subject: "",
      description: "",
      categoryId: undefined,
    },
  });

  function onSubmit(data: CreateTicketFormData) {
    mutation.mutate(
      { request: data, attachments: selectedFiles },
      {
        onSuccess: (ticket) => {
          message.success("Ticket created successfully.");
          router.push(`/tickets/${ticket.id}`);
        },
      },
    );
  }

  function beforeUpload(file: File) {
    setAttachmentError(null);
    const extension = file.name.slice(file.name.lastIndexOf(".")).toLowerCase();

    if (file.size === 0) {
      setAttachmentError("Empty files cannot be attached.");
      return Upload.LIST_IGNORE;
    }

    if (file.size > MAX_ATTACHMENT_FILE_SIZE) {
      setAttachmentError(`${file.name} exceeds the 10 MB limit.`);
      return Upload.LIST_IGNORE;
    }

    if (!ACCEPTED_ATTACHMENT_EXTENSIONS.includes(extension)) {
      setAttachmentError(`${file.name} has an unsupported file extension.`);
      return Upload.LIST_IGNORE;
    }

    if (selectedFilesRef.current.length >= MAX_TICKET_ATTACHMENTS) {
      setAttachmentError(
        `A ticket can have at most ${MAX_TICKET_ATTACHMENTS} attachments.`,
      );
      return Upload.LIST_IGNORE;
    }

    const updatedFiles = [...selectedFilesRef.current, file];
    selectedFilesRef.current = updatedFiles;
    setSelectedFiles(updatedFiles);
    return Upload.LIST_IGNORE;
  }

  function removeSelectedFile(index: number) {
    const updatedFiles = selectedFilesRef.current.filter(
      (_, fileIndex) => fileIndex !== index,
    );
    selectedFilesRef.current = updatedFiles;
    setSelectedFiles(updatedFiles);
    setAttachmentError(null);
  }

  return (
    <FormCard
      title="Create Ticket"
      description="Submit a new support request."
      onSubmit={handleSubmit(onSubmit)}
    >
      <div>
        <FormLabel>Category</FormLabel>
        <FormSelect
          control={control}
          name="categoryId"
          placeholder="Select a category (optional)"
          loading={isLoadingCategories}
          options={categories.map((category) => ({
            label: category.name,
            value: category.id,
          }))}
        />
      </div>

      {categoriesError && (
        <Alert
          type="error"
          showIcon
          message="Unable to load ticket categories. You can still create this ticket without a category."
        />
      )}

      <div>
        <FormLabel>Subject</FormLabel>
        <FormInput
          control={control}
          name="subject"
          placeholder="Unable to log in"
          maxLength={TICKET_VALIDATION.SUBJECT_MAX_LENGTH}
        />
      </div>

      <div>
        <FormLabel>Description</FormLabel>
        <FormTextArea
          control={control}
          name="description"
          placeholder="Describe your issue..."
          rows={8}
          maxLength={TICKET_VALIDATION.DESCRIPTION_MAX_LENGTH}
        />
      </div>

      <div>
        <FormLabel>Attachments</FormLabel>
        <Upload.Dragger
          accept={ACCEPTED_ATTACHMENT_EXTENSIONS.join(",")}
          beforeUpload={beforeUpload}
          multiple
          showUploadList={false}
          disabled={mutation.isPending}
        >
          <div className="flex flex-col items-center gap-2 py-2">
            <Paperclip className="text-slate-500" aria-hidden="true" />
            <Typography.Text>
              Click or drag files here to attach
            </Typography.Text>
            <Typography.Text type="secondary">
              Up to {MAX_TICKET_ATTACHMENTS} files, 10 MB each. PDF, images,
              documents, spreadsheets, or CSV.
            </Typography.Text>
          </div>
        </Upload.Dragger>

        {attachmentError && (
          <Alert
            className="mt-3"
            type="error"
            showIcon
            message={attachmentError}
          />
        )}

        {selectedFiles.length > 0 && (
          <ul className="mt-3 divide-y divide-slate-200 rounded-md border border-slate-200">
            {selectedFiles.map((file, index) => (
              <li
                key={`${file.name}-${file.size}-${file.lastModified}-${index}`}
                className="flex items-center justify-between gap-3 p-3"
              >
                <div className="min-w-0">
                  <p className="truncate text-sm font-medium text-slate-900">
                    {file.name}
                  </p>
                  <p className="text-xs text-slate-500">
                    {formatFileSize(file.size)}
                  </p>
                </div>
                <AntButton
                  type="text"
                  danger
                  aria-label={`Remove ${file.name}`}
                  icon={<Trash2 size={16} aria-hidden="true" />}
                  onClick={() => removeSelectedFile(index)}
                  disabled={mutation.isPending}
                />
              </li>
            ))}
          </ul>
        )}
      </div>

      {mutation.error && <FormError error={mutation.error} />}

      <Button
        htmlType="submit"
        className="w-full"
        isLoading={mutation.isPending}
      >
        Create Ticket
      </Button>
    </FormCard>
  );
}

function formatFileSize(fileSize: number) {
  if (fileSize < 1024) return `${fileSize} B`;
  if (fileSize < 1024 * 1024) return `${(fileSize / 1024).toFixed(1)} KB`;
  return `${(fileSize / (1024 * 1024)).toFixed(1)} MB`;
}
