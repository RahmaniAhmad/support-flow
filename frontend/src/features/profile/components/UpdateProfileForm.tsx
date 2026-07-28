"use client";

import { AxiosError } from "axios";
import { zodResolver } from "@hookform/resolvers/zod";
import { useForm } from "react-hook-form";

import Button from "@/components/ui/Button";
import FormInput from "@/components/form/FormInput";
import { useMessage } from "@/app/providers/MessageProvider";

import { useUpdateProfile } from "../hooks/useUpdateProfile";
import {
  updateProfileSchema,
  UpdateProfileFormData,
} from "../schemas/update-profile.schema";
import { useProfile } from "../hooks/useProfile";
import { useEffect } from "react";
import { useRouter } from "next/navigation";
import FormCard from "@/components/form/FormCard";

export default function UpdateProfileForm() {
  const router = useRouter();

  const message = useMessage();

  const { data: profile, isLoading: isProfileLoading } = useProfile();

  const mutation = useUpdateProfile();

  const { control, handleSubmit, reset } = useForm<UpdateProfileFormData>({
    resolver: zodResolver(updateProfileSchema),
    defaultValues: {
      firstName: "",
      lastName: "",
      phone: "",
    },
  });

  useEffect(() => {
    if (profile) {
      reset({
        firstName: profile.firstName,
        lastName: profile.lastName,
        phone: profile.phone ?? "",
      });
    }
  }, [profile, reset]);

  async function onSubmit(data: UpdateProfileFormData) {
    try {
      await mutation.mutateAsync(data);

      message.success("Profile updated successfully.");

      router.push("/profile");
    } catch {
      // React Query exposes the error through mutation.error
    }
  }

  const error =
    mutation.error instanceof AxiosError
      ? (mutation.error.response?.data?.message ?? "Failed to update profile.")
      : mutation.isError
        ? "Failed to update profile."
        : null;

  if (isProfileLoading) {
    return (
      <div className="max-w-2xl rounded-xl bg-white p-6 shadow">
        Loading profile...
      </div>
    );
  }

  return (
    <FormCard
      title="Edit Profile"
      description="Update your personal information."
      onSubmit={handleSubmit(onSubmit)}
    >
      <div>
        <label
          htmlFor="firstName"
          className="mb-2 block text-sm font-medium text-slate-700"
        >
          First Name
        </label>

        <FormInput control={control} name="firstName" placeholder="Ahmad" />
      </div>

      <div>
        <label
          htmlFor="lastName"
          className="mb-2 block text-sm font-medium text-slate-700"
        >
          Last Name
        </label>

        <FormInput control={control} name="lastName" placeholder="Rahmani" />
      </div>

      <div>
        <label
          htmlFor="phone"
          className="mb-2 block text-sm font-medium text-slate-700"
        >
          Phone
        </label>

        <FormInput
          control={control}
          name="phone"
          placeholder="+98 912 000 0000"
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
        Update Profile
      </Button>
    </FormCard>
  );
}
