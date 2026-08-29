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
import { useMessage } from "@/app/providers/MessageProvider";
import FormCard from "@/components/form/FormCard";
import FormLabel from "@/components/form/FormLabel";
import { TICKET_VALIDATION } from "../constants/ticket-validation";
import FormError from "@/components/form/FormError";

export default function CreateTicketForm() {
  const router = useRouter();

  const message = useMessage();

  const mutation = useCreateTicket();

  const { control, handleSubmit } = useForm<CreateTicketFormData>({
    resolver: zodResolver(createTicketSchema),
    defaultValues: {
      subject: "",
      description: "",
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
