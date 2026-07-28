"use client";

import { useState } from "react";
import { Modal } from "antd";
import { AxiosError } from "axios";
import { zodResolver } from "@hookform/resolvers/zod";
import { useForm } from "react-hook-form";

import Button from "@/components/ui/Button";
import FormInput from "@/components/form/FormInput";

import { useMessage } from "@/app/providers/MessageProvider";

import {
  resetPasswordSchema,
  ResetPasswordFormData,
} from "../schemas/reset-password.schema";

import { useResetUserPassword } from "../hooks/useResetUserPassword";

type Props = {
  userId: string;
};

export default function ResetPasswordButton({ userId }: Props) {
  const [open, setOpen] = useState(false);

  const message = useMessage();

  const mutation = useResetUserPassword(userId);

  const { control, handleSubmit, reset } = useForm<ResetPasswordFormData>({
    resolver: zodResolver(resetPasswordSchema),

    defaultValues: {
      password: "",
    },
  });

  function handleClose() {
    reset();
    setOpen(false);
  }

  async function onSubmit(data: ResetPasswordFormData) {
    try {
      await mutation.mutateAsync(data.password);

      message.success("Password reset successfully.");

      handleClose();
    } catch {
      // error handled below
    }
  }

  const error =
    mutation.error instanceof AxiosError
      ? (mutation.error.response?.data?.message ?? "Failed to reset password.")
      : mutation.isError
        ? "Failed to reset password."
        : null;

  return (
    <>
      <Button onClick={() => setOpen(true)}>Reset Password</Button>

      <Modal
        title="Reset Password"
        open={open}
        onCancel={handleClose}
        footer={null}
        destroyOnHidden
      >
        <form
          noValidate
          onSubmit={handleSubmit(onSubmit)}
          className="mt-4 flex flex-col gap-y-4"
        >
          <div>
            <label
              className="
                mb-2
                block
                text-sm
                font-medium
                text-slate-700
              "
            >
              New Password
            </label>

            <FormInput
              control={control}
              name="password"
              type="password"
              placeholder="Enter new password"
            />
          </div>

          {error && (
            <div
              className="
                rounded-md
                border
                border-red-200
                bg-red-50
                px-3
                py-2
                text-sm
                text-red-700
              "
            >
              {error}
            </div>
          )}

          <Button
            htmlType="submit"
            className="w-full"
            isLoading={mutation.isPending}
          >
            Reset Password
          </Button>
        </form>
      </Modal>
    </>
  );
}
