"use client";

import { useRouter } from "next/navigation";
import { AxiosError } from "axios";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";

import Button from "@/components/ui/Button";
import FormInput from "@/components/form/FormInput";

import { useMessage } from "@/app/providers/MessageProvider";

import {
  updateUserSchema,
  UpdateUserFormData,
} from "../schemas/update-user.schema";

import { useUpdateUser } from "../hooks/useUpdateUser";
import FormCard from "@/components/form/FormCard";

type Props = {
  initialValues: UpdateUserFormData;
  userId: string;
};

export default function UpdateUserForm({ initialValues, userId }: Props) {
  const router = useRouter();

  const message = useMessage();

  const mutation = useUpdateUser(userId);

  const { control, handleSubmit } = useForm<UpdateUserFormData>({
    resolver: zodResolver(updateUserSchema),

    defaultValues: initialValues ?? {
      firstName: "",
      lastName: "",
      phone: "",
    },
  });

  async function onSubmit(data: UpdateUserFormData) {
    try {
      await mutation.mutateAsync(data);

      message.success("User updated successfully.");

      router.push(`/users/${userId}`);
    } catch {}
  }

  const error =
    mutation.error instanceof AxiosError
      ? (mutation.error.response?.data?.message ?? "Failed to update user.")
      : mutation.isError
        ? "Failed to update user."
        : null;

  return (
    <FormCard
      title="Edit User"
      description="Update user information."
      onSubmit={handleSubmit(onSubmit)}
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
          First Name
        </label>

        <FormInput control={control} name="firstName" placeholder="John" />
      </div>

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
          Last Name
        </label>

        <FormInput control={control} name="lastName" placeholder="Smith" />
      </div>

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
          Phone
        </label>

        <FormInput control={control} name="phone" placeholder="+123456789" />
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
        Save Changes
      </Button>
    </FormCard>
  );
}
