"use client";

import { useState } from "react";
import { Modal } from "antd";
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
import FormError from "@/components/form/FormError";
import FormLabel from "@/components/form/FormLabel";

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

  function onSubmit(data: ResetPasswordFormData) {
    mutation.mutate(data.password, {
      onSuccess: () => {
        message.success("Password reset successfully.");
        handleClose();
      },
    });
  }

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
            <FormLabel>New Password</FormLabel>

            <FormInput
              control={control}
              name="password"
              type="password"
              placeholder="Enter new password"
            />
          </div>

          {mutation.error && <FormError error={mutation.error} />}

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
