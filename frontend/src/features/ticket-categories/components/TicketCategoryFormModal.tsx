"use client";

import { zodResolver } from "@hookform/resolvers/zod";
import { Alert, Modal } from "antd";
import { useEffect } from "react";
import { useForm } from "react-hook-form";
import FormInput from "@/components/form/FormInput";
import FormLabel from "@/components/form/FormLabel";
import FormTextArea from "@/components/form/FormTextArea";
import { useMessage } from "@/app/providers/MessageProvider";
import {
  useCreateTicketCategory,
  useUpdateTicketCategory,
} from "../hooks/useTicketCategories";
import {
  TicketCategoryFormData,
  ticketCategorySchema,
} from "../schemas/ticketCategory.schema";
import { TicketCategory } from "../types";

type Props = {
  open: boolean;
  category?: TicketCategory;
  onClose: () => void;
};

export default function TicketCategoryFormModal({
  open,
  category,
  onClose,
}: Props) {
  const message = useMessage();
  const createMutation = useCreateTicketCategory();
  const updateMutation = useUpdateTicketCategory();
  const mutation = category ? updateMutation : createMutation;
  const { control, handleSubmit, reset } = useForm<TicketCategoryFormData>({
    resolver: zodResolver(ticketCategorySchema),
    defaultValues: {
      name: category?.name ?? "",
      description: category?.description ?? "",
    },
  });

  useEffect(() => {
    reset({
      name: category?.name ?? "",
      description: category?.description ?? "",
    });
  }, [category, reset, open]);

  function onSubmit(data: TicketCategoryFormData) {
    const request = {
      name: data.name,
      description: data.description?.trim() || undefined,
    };
    const onSuccess = () => {
      message.success(
        category
          ? "Ticket category updated."
          : "Ticket category created.",
      );
      onClose();
    };

    if (category) {
      updateMutation.mutate(
        { id: category.id, request },
        { onSuccess },
      );
    } else {
      createMutation.mutate(request, { onSuccess });
    }
  }

  return (
    <Modal
      title={category ? "Edit ticket category" : "Create ticket category"}
      open={open}
      onCancel={onClose}
      onOk={handleSubmit(onSubmit)}
      okText={category ? "Save changes" : "Create category"}
      confirmLoading={mutation.isPending}
      destroyOnHidden
    >
      <form className="flex flex-col gap-4" onSubmit={handleSubmit(onSubmit)}>
        {mutation.error && (
          <Alert
            type="error"
            showIcon
            message="Unable to save the ticket category. Check for duplicate names and try again."
          />
        )}
        <div>
          <FormLabel>Name</FormLabel>
          <FormInput
            control={control}
            name="name"
            placeholder="Billing"
            maxLength={100}
          />
        </div>
        <div>
          <FormLabel>Description</FormLabel>
          <FormTextArea
            control={control}
            name="description"
            placeholder="Optional details"
            rows={3}
            maxLength={500}
          />
        </div>
      </form>
    </Modal>
  );
}
