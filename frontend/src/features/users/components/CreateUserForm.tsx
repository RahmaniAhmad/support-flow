"use client";

import { useRouter } from "next/navigation";
import { AxiosError } from "axios";
import { zodResolver } from "@hookform/resolvers/zod";
import { useForm } from "react-hook-form";

import Button from "@/components/ui/Button";
import FormInput from "@/components/form/FormInput";
import PageTitle from "@/components/ui/page/PageTitle";
import PageDescription from "@/components/ui/page/PageDescription";
import { useMessage } from "@/app/providers/MessageProvider";

import { useCreateUser } from "../hooks/useCreateUser";
import {
  createUserSchema,
  CreateUserFormData,
} from "../schemas/create-user.schema";

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

  async function onSubmit(data: CreateUserFormData) {
    try {
      await mutation.mutateAsync(data);

      message.success("User created successfully.");

      router.push("/users");
    } catch {
      // error handled below
    }
  }

  const error =
    mutation.error instanceof AxiosError
      ? (mutation.error.response?.data?.message ?? "Failed to create user.")
      : mutation.isError
        ? "Failed to create user."
        : null;

  return (
    <form
      noValidate
      onSubmit={handleSubmit(onSubmit)}
      className="
        max-w-2xl
        rounded-xl
        bg-white
        p-6
        shadow
      "
    >
      <div className="mt-3 mb-6">
        <PageTitle>Create User</PageTitle>

        <PageDescription>Add a new user to your company.</PageDescription>
      </div>

      <div className="flex flex-col gap-y-4">
        <div>
          <label
            htmlFor="firstName"
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
            htmlFor="lastName"
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
            htmlFor="email"
            className="
              mb-2
              block
              text-sm
              font-medium
              text-slate-700
            "
          >
            Email
          </label>

          <FormInput
            control={control}
            name="email"
            placeholder="john@example.com"
          />
        </div>

        <div>
          <label
            htmlFor="password"
            className="
              mb-2
              block
              text-sm
              font-medium
              text-slate-700
            "
          >
            Password
          </label>

          <FormInput
            control={control}
            name="password"
            type="password"
            placeholder="********"
          />
        </div>

        <div>
          <label
            htmlFor="phone"
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

        <div>
          <label
            htmlFor="role"
            className="
              mb-2
              block
              text-sm
              font-medium
              text-slate-700
            "
          >
            Role
          </label>

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
          Create User
        </Button>
      </div>
    </form>
  );
}
