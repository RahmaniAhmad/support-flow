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
import { Alert } from "antd";
import { useMessage } from "@/app/providers/MessageProvider";
import FormCard from "@/components/form/FormCard";
import FormLabel from "@/components/form/FormLabel";
import { TICKET_VALIDATION } from "../constants/ticket-validation";
import FormError from "@/components/form/FormError";
import FormSelect from "@/components/form/FormSelect";
import { useActiveTicketCategories } from "@/features/ticket-categories/hooks/useTicketCategories";

export default function CreateTicketForm() {
  const router = useRouter();

  const message = useMessage();

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
    mutation.mutate(data, {
      onSuccess: () => {
        message.success("Ticket created successfully.");
        router.push("/tickets");
      },
    });
  }

  return (
    <FormCard
      title="Create Ticket"
      description="Submit a new support request."
      onSubmit={handleSubmit(onSubmit)}
    >
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
