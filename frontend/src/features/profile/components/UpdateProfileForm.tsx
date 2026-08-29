"use client";

import { zodResolver } from "@hookform/resolvers/zod";
import { useForm } from "react-hook-form";

import { useMessage } from "@/app/providers/MessageProvider";

import { useUpdateProfile } from "../hooks/useUpdateProfile";
import {
  updateProfileSchema,
  UpdateProfileFormData,
} from "../schemas/update-profile.schema";
import { useRouter } from "next/navigation";
import FormCard from "@/components/form/FormCard";
import FormInput from "@/components/form/FormInput";
import Button from "@/components/ui/Button";
import FormLabel from "@/components/form/FormLabel";
import { PROFILE_VALIDATION } from "../constants/profile-validation";
import FormError from "@/components/form/FormError";

type Props = {
  profile: UpdateProfileFormData;
};

export default function UpdateProfileForm({ profile }: Props) {
  const router = useRouter();

  const message = useMessage();

  const mutation = useUpdateProfile();

  const { control, handleSubmit } = useForm<UpdateProfileFormData>({
    resolver: zodResolver(updateProfileSchema),
    defaultValues: profile ?? {
      firstName: "",
      lastName: "",
      phone: "",
    },
  });

  function onSubmit(data: UpdateProfileFormData) {
    mutation.mutate(data, {
      onSuccess: () => {
        message.success("Profile updated successfully.");
        router.push("/profile");
      },
    });
  }

  return (
    <FormCard
      title="Edit Profile"
      description="Update your personal information."
      onSubmit={handleSubmit(onSubmit)}
    >
      <div>
        <FormLabel>First Name</FormLabel>
        <FormInput
          control={control}
          name="firstName"
          placeholder="Ahmad"
          maxLength={PROFILE_VALIDATION.FIRST_NAME_MAX_LENGTH}
        />
      </div>

      <div>
        <FormLabel>Last Name</FormLabel>
        <FormInput
          control={control}
          name="lastName"
          placeholder="Rahmani"
          maxLength={PROFILE_VALIDATION.LAST_NAME_MAX_LENGTH}
        />
      </div>

      <div>
        <FormLabel>Phone</FormLabel>
        <FormInput
          control={control}
          name="phone"
          placeholder="+98 912 000 0000"
          maxLength={PROFILE_VALIDATION.PHONE_MAX_LENGTH}
        />
      </div>

      {mutation.error && <FormError error={mutation.error} />}

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
