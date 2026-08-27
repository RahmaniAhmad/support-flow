"use client";

import { useRouter } from "next/navigation";
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
import { USER_VALIDATION } from "../constants/user-validation";
import FormError from "@/components/form/FormError";
import FormLabel from "@/components/form/FormLabel";

type Props = {
  user: UpdateUserFormData;
  userId: string;
};

export default function UpdateUserForm({ user, userId }: Props) {
  const router = useRouter();

  const message = useMessage();

  const mutation = useUpdateUser(userId);

  const { control, handleSubmit } = useForm<UpdateUserFormData>({
    resolver: zodResolver(updateUserSchema),

    defaultValues: user ?? {
      firstName: "",
      lastName: "",
      phone: "",
    },
  });

  function onSubmit(data: UpdateUserFormData) {
    mutation.mutate(data, {
      onSuccess: () => {
        message.success("User updated successfully.");
        router.push(`/users/${userId}`);
      },
    });
  }

  return (
    <FormCard
      title="Edit User"
      description="Update user information."
      onSubmit={handleSubmit(onSubmit)}
    >
      <div>
        <FormLabel>First Name</FormLabel>
        <FormInput
          control={control}
          name="firstName"
          placeholder="Ahmad"
          maxLength={USER_VALIDATION.FIRST_NAME_MAX_LENGTH}
        />
      </div>

      <div>
        <FormLabel>Last Name</FormLabel>
        <FormInput
          control={control}
          name="lastName"
          placeholder="Rahmani"
          maxLength={USER_VALIDATION.LAST_NAME_MAX_LENGTH}
        />
      </div>

      <div>
        <FormLabel>Phone</FormLabel>
        <FormInput
          control={control}
          name="phone"
          placeholder="+123456789"
          maxLength={USER_VALIDATION.PHONE_MAX_LENGTH}
        />
      </div>

      {mutation.error && <FormError error={mutation.error} />}

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
