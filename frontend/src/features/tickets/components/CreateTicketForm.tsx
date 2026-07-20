"use client";

import { useRouter } from "next/navigation";
import { AxiosError } from "axios";
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
import PageTitle from "@/components/ui/page/PageTitle";
import PageDescription from "@/components/ui/page/PageDescription";

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

  async function onSubmit(data: CreateTicketFormData) {
    try {
      await mutation.mutateAsync(data);

      message.success("Ticket created successfully.");

      router.push("/tickets");
    } catch {
      // React Query exposes the error through mutation.error
    }
  }

  const error =
    mutation.error instanceof AxiosError
      ? (mutation.error.response?.data?.message ?? "Failed to create ticket.")
      : mutation.isError
        ? "Failed to create ticket."
        : null;

  return (
    <form
      noValidate
      onSubmit={handleSubmit(onSubmit)}
      className="max-w-2xl rounded-xl bg-white p-6 shadow"
    >
      <div className="mt-3 mb-6">
        <PageTitle>Create Ticket</PageTitle>

        <PageDescription>Submit a new support request.</PageDescription>
      </div>

      <div className="flex flex-col gap-y-4">
        <div>
          <label
            htmlFor="subject"
            className="mb-2 block text-sm font-medium text-slate-700"
          >
            Subject
          </label>

          <FormInput
            control={control}
            name="subject"
            placeholder="Unable to log in"
          />
        </div>

        <div>
          <label
            htmlFor="description"
            className="mb-2 block text-sm font-medium text-slate-700"
          >
            Description
          </label>

          <FormTextArea
            control={control}
            name="description"
            placeholder="Describe your issue..."
            rows={8}
          />
        </div>

        {error && (
          <div className="rounded-md border border-red-200 bg-red-50 px-3 py-2 text-sm text-red-700">
            {error}
          </div>
        )}

        <Button
          htmlType="submit"
          className="w-full"
          isLoading={mutation.isPending}
        >
          Create Ticket
        </Button>
      </div>
    </form>
  );
}
