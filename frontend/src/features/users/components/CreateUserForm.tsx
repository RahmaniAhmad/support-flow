"use client";

import { useRouter } from "next/navigation";
import { zodResolver } from "@hookform/resolvers/zod";
import { useForm } from "react-hook-form";

import Button from "@/components/ui/Button";
import FormInput from "@/components/form/FormInput";
import { useMessage } from "@/app/providers/MessageProvider";

import { useCreateUser } from "../hooks/useCreateUser";
import {
  createUserSchema,
  CreateUserFormData,
} from "../schemas/create-user.schema";
import FormCard from "@/components/form/FormCard";
import FormError from "@/components/form/FormError";
import { USER_VALIDATION } from "../constants/user-validation";
import FormLabel from "@/components/form/FormLabel";

export default function CreateUserForm() {
  const router = useRouter();
  const message = useMessage();

  const mutation = useCreateUser();

  const { control, handleSubmit } = useForm<CreateUserFormData>({
    resolver: zodResolver(createUserSchema),

    defaultValues: {
      firstName: "",
      lastName: "",
      email: "",
      password: "",
      phone: "",
      role: "Agent",
    },
  });

  function onSubmit(data: CreateUserFormData) {
    mutation.mutate(data, {
      onSuccess: () => {
        message.success("User created successfully.");
        router.push("/users");
      },
    });
  }

  return (
    <FormCard
      title="Create User"
      description="Add a new user to your company."
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
        <FormLabel>Email</FormLabel>
        <FormInput
          control={control}
          name="email"
          placeholder="ahmad@example.com"
          maxLength={USER_VALIDATION.EMAIL_MAX_LENGTH}
        />
      </div>

      <div>
        <FormLabel>Password</FormLabel>
        <FormInput
          control={control}
          name="password"
          type="password"
          placeholder="********"
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

      <div>
        <FormLabel>Role</FormLabel>
        <select
          {...control.register("role")}
          className="
              h-10
              w-full
              rounded-md
              border
              border-slate-300
              px-3
              text-sm
            "
        >
          <option value="Agent">Agent</option>

          <option value="Customer">Customer</option>
        </select>
      </div>

      {mutation.error && <FormError error={mutation.error} />}

      <Button
        htmlType="submit"
        className="w-full"
        isLoading={mutation.isPending}
      >
        Create User
      </Button>
    </FormCard>
  );
}
