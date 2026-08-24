"use client";

import Link from "next/link";
import { useSearchParams } from "next/navigation";
import { useState } from "react";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";

import AuthLayout from "@/features/auth/components/AuthLayout";
import AuthHeader from "@/features/auth/components/AuthHeader";
import AuthError from "@/features/auth/components/AuthError";

import Button from "@/components/ui/Button";
import FormPasswordInput from "@/components/form/FormPasswordInput";

import { useResetPassword } from "@/features/auth/hooks/useResetPassword";

import {
  resetPasswordSchema,
  ResetPasswordForm,
} from "@/features/auth/schemas/resetPassword.schema";
import { getProblemDetails } from "@/lib/api/errors";

export default function ResetPasswordView() {
  const searchParams = useSearchParams();

  const token = searchParams.get("token");

  const resetPasswordMutation = useResetPassword();

  const [success, setSuccess] = useState(false);

  const { control, handleSubmit } = useForm<ResetPasswordForm>({
    resolver: zodResolver(resetPasswordSchema),
    defaultValues: {
      password: "",
      confirmPassword: "",
    },
  });

  async function onSubmit(data: ResetPasswordForm) {
    if (!token) {
      return;
    }

    await resetPasswordMutation.mutateAsync({
      token,
      password: data.password,
    });

    setSuccess(true);
  }

  const problemDetails = getProblemDetails(resetPasswordMutation.error);

  const error = resetPasswordMutation.isError
    ? (problemDetails?.detail ?? "The reset link is invalid or has expired.")
    : null;

  if (!token) {
    return (
      <AuthLayout>
        <div className="rounded-2xl border border-slate-200 bg-white p-6 text-center shadow-sm sm:p-8">
          <h1 className="text-2xl font-bold text-slate-900">
            Invalid reset link
          </h1>

          <p className="mt-3 text-sm leading-6 text-slate-500">
            This password reset link is missing or invalid.
          </p>

          <Link
            href="/forgot-password"
            className="mt-6 inline-flex font-semibold text-blue-600 hover:text-blue-700"
          >
            Request a new reset link
          </Link>
        </div>
      </AuthLayout>
    );
  }

  if (success) {
    return (
      <AuthLayout>
        <div className="rounded-2xl border border-slate-200 bg-white p-6 text-center shadow-sm sm:p-8">
          <div className="mx-auto mb-5 flex h-12 w-12 items-center justify-center rounded-full bg-green-100">
            <svg
              className="h-6 w-6 text-green-600"
              fill="none"
              viewBox="0 0 24 24"
              stroke="currentColor"
              strokeWidth={2}
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                d="M5 13l4 4L19 7"
              />
            </svg>
          </div>

          <h1 className="text-2xl font-bold text-slate-900">
            Password updated
          </h1>

          <p className="mt-3 text-sm leading-6 text-slate-500">
            Your password has been successfully updated. You can now sign in
            with your new password.
          </p>

          <Link
            href="/login"
            className="mt-6 inline-flex font-semibold text-blue-600 hover:text-blue-700"
          >
            Back to sign in
          </Link>
        </div>
      </AuthLayout>
    );
  }

  return (
    <AuthLayout>
      <div className="rounded-2xl border border-slate-200 bg-white p-6 shadow-sm sm:p-8">
        <AuthHeader
          title="Create a new password"
          description="Choose a strong password for your SupportFlow account."
        />

        <form
          noValidate
          onSubmit={handleSubmit(onSubmit)}
          className="space-y-5"
        >
          <div>
            <label
              htmlFor="password"
              className="mb-2 block text-sm font-medium text-slate-700"
            >
              New password
            </label>

            <FormPasswordInput
              control={control}
              name="password"
              placeholder="Enter your new password"
            />
          </div>

          <div>
            <label
              htmlFor="confirmPassword"
              className="mb-2 block text-sm font-medium text-slate-700"
            >
              Confirm new password
            </label>

            <FormPasswordInput
              control={control}
              name="confirmPassword"
              placeholder="Confirm your new password"
            />
          </div>

          <AuthError message={error} />

          <Button
            className="w-full"
            htmlType="submit"
            isLoading={resetPasswordMutation.isPending}
          >
            Update Password
          </Button>
        </form>

        <p className="mt-6 text-center text-sm text-slate-600">
          Remember your password?{" "}
          <Link
            href="/login"
            className="font-semibold text-blue-600 hover:text-blue-700"
          >
            Sign in
          </Link>
        </p>
      </div>
    </AuthLayout>
  );
}
