"use client";

import { AxiosError } from "axios";
import { zodResolver } from "@hookform/resolvers/zod";
import { useForm } from "react-hook-form";

import Button from "@/components/ui/Button";
import { useMessage } from "@/app/providers/MessageProvider";

import { useAssignTicket } from "../hooks/useAssignTicket";
import { useAssignableUsers } from "../hooks/useAssignableUsers";
import {
  AssignTicketFormData,
  assignTicketSchema,
} from "../schemas/assignTicket.schema";
import FormSelect from "@/components/form/FormSelect";
import FormCard from "@/components/form/FormCard";

type Props = {
  ticketId: string;
  onSuccess: () => void;
};

export default function AssignTicketForm({ ticketId, onSuccess }: Props) {
  const message = useMessage();

  const { data: users = [], isLoading } = useAssignableUsers();

  const mutation = useAssignTicket();

  const { control, handleSubmit } = useForm<AssignTicketFormData>({
    resolver: zodResolver(assignTicketSchema),

    defaultValues: {
      assignedToUserId: "",
    },
  });

  async function onSubmit(data: AssignTicketFormData) {
    try {
      await mutation.mutateAsync({
        ticketId,
        request: {
          assignedToUserId: data.assignedToUserId,
        },
      });

      onSuccess();

      message.success("Ticket assigned successfully.");
    } catch {
      //
    }
  }

  const error =
    mutation.error instanceof AxiosError
      ? (mutation.error.response?.data?.message ?? "Failed to assign ticket.")
      : mutation.isError
        ? "Failed to assign ticket."
        : null;

  return (
    <FormCard onSubmit={handleSubmit(onSubmit)}>
      <FormSelect
        control={control}
        name="assignedToUserId"
        label="Assign to"
        placeholder="Select a user"
        loading={isLoading}
        options={users.map((user) => ({
          label: user.fullName,
          value: user.id,
        }))}
      />

      {error && (
        <div
          className="
            mt-3 rounded-md
            border border-red-200
            bg-red-50 px-3 py-2
            text-sm text-red-700
          "
        >
          {error}
        </div>
      )}

      <Button htmlType="submit" className="mt-4" isLoading={mutation.isPending}>
        Assign Ticket
      </Button>
    </FormCard>
  );
}
