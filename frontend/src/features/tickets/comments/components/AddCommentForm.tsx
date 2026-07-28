"use client";

import { AxiosError } from "axios";
import { zodResolver } from "@hookform/resolvers/zod";
import { useForm } from "react-hook-form";

import Button from "@/components/ui/Button";
import FormTextArea from "@/components/form/FormTextArea";
import { useMessage } from "@/app/providers/MessageProvider";

import {
  addCommentSchema,
  AddCommentFormData,
} from "../schemas/addComment.schema";

import { useAddComment } from "../hooks/useAddComment";
import FormCard from "@/components/form/FormCard";

type Props = {
  ticketId: string;
  onSuccess: () => void;
};

export default function AddCommentForm({ ticketId, onSuccess }: Props) {
  const message = useMessage();

  const mutation = useAddComment();

  const { control, handleSubmit, reset } = useForm<AddCommentFormData>({
    resolver: zodResolver(addCommentSchema),

    defaultValues: {
      content: "",
    },
  });

  async function onSubmit(data: AddCommentFormData) {
    try {
      await mutation.mutateAsync({
        ticketId,
        request: {
          content: data.content,
        },
      });

      reset();

      onSuccess();

      message.success("Comment added successfully.");
    } catch {
      //
    }
  }

  const error =
    mutation.error instanceof AxiosError
      ? (mutation.error.response?.data?.message ?? "Failed to add comment.")
      : mutation.isError
        ? "Failed to add comment."
        : null;

  return (
    <FormCard onSubmit={handleSubmit(onSubmit)}>
      <FormTextArea
        control={control}
        name="content"
        rows={4}
        placeholder="Write a comment..."
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
        Add Comment
      </Button>
    </FormCard>
  );
}
