"use client";

import Link from "next/link";
import { useRouter } from "next/navigation";
import { AxiosError } from "axios";
import { zodResolver } from "@hookform/resolvers/zod";
import { useForm } from "react-hook-form";

import Button from "@/components/ui/Button";
import Input from "@/components/ui/Input";

import { useRegister } from "@/features/auth/hooks/useRegister";
import {
  registerSchema,
  RegisterForm,
} from "@/features/auth/schemas/register.schema";

export default function RegisterPage() {
  const router = useRouter();

  const registerMutation = useRegister();

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<RegisterForm>({
    resolver: zodResolver(registerSchema),
    defaultValues: {
      companyName: "",
      email: "",
      password: "",
      confirmPassword: "",
    },
  });

  async function onSubmit(data: RegisterForm) {
    const { confirmPassword, ...request } = data;

    try {
      await registerMutation.mutateAsync(request);

      router.push("/login");
    } catch {
      // React Query exposes the error through registerMutation.error
    }
  }

  const error =
    registerMutation.error instanceof AxiosError
      ? (registerMutation.error.response?.data?.message ??
        "Registration failed.")
      : registerMutation.isError
        ? "Registration failed."
        : null;

  return (
    <main className="flex min-h-screen items-center justify-center bg-slate-100 px-4">
      <form
        noValidate
        onSubmit={handleSubmit(onSubmit)}
        className="w-full max-w-md rounded-xl bg-white p-8 shadow-lg"
      >
        <h1 className="mb-2 text-center text-3xl font-bold text-slate-900">
          Create Company
        </h1>

        <p className="mb-8 text-center text-sm text-slate-500">
          Register your company to start managing tickets.
        </p>

        <div className="space-y-5">
          <div>
            <label
              htmlFor="companyName"
              className="mb-2 block text-sm font-medium text-slate-700"
            >
              Company Name
            </label>

            <Input
              id="companyName"
              placeholder="Acme Inc."
              {...register("companyName")}
            />

            {errors.companyName && (
              <p className="mt-1 text-sm text-red-600">
                {errors.companyName.message}
              </p>
            )}
          </div>

          <div>
            <label
              htmlFor="email"
              className="mb-2 block text-sm font-medium text-slate-700"
            >
              Email
            </label>

            <Input
              id="email"
              type="email"
              placeholder="admin@company.com"
              {...register("email")}
            />

            {errors.email && (
              <p className="mt-1 text-sm text-red-600">
                {errors.email.message}
              </p>
            )}
          </div>

          <div>
            <label
              htmlFor="password"
              className="mb-2 block text-sm font-medium text-slate-700"
            >
              Password
            </label>

            <Input
              id="password"
              type="password"
              placeholder="••••••••"
              {...register("password")}
            />

            {errors.password && (
              <p className="mt-1 text-sm text-red-600">
                {errors.password.message}
              </p>
            )}
          </div>

          <div>
            <label
              htmlFor="confirmPassword"
              className="mb-2 block text-sm font-medium text-slate-700"
            >
              Confirm Password
            </label>

            <Input
              id="confirmPassword"
              type="password"
              placeholder="••••••••"
              {...register("confirmPassword")}
            />

            {errors.confirmPassword && (
              <p className="mt-1 text-sm text-red-600">
                {errors.confirmPassword.message}
              </p>
            )}
          </div>

          {error && (
            <div className="rounded-md border border-red-200 bg-red-50 px-3 py-2 text-sm text-red-700">
              {error}
            </div>
          )}

          <Button type="submit" isLoading={registerMutation.isPending}>
            Create Company
          </Button>
        </div>

        <p className="mt-8 text-center text-sm text-slate-600">
          Already have an account?{" "}
          <Link
            href="/login"
            className="font-medium text-blue-600 hover:text-blue-700"
          >
            Sign in
          </Link>
        </p>
      </form>
    </main>
  );
}
