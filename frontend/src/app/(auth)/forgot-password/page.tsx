"use client";

import Link from "next/link";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";

import AuthLayout from "@/features/auth/components/AuthLayout";
import AuthHeader from "@/features/auth/components/AuthHeader";
import AuthError from "@/features/auth/components/AuthError";

import Button from "@/components/ui/Button";
import FormInput from "@/components/form/FormInput";

import { useForgotPassword } from "@/features/auth/hooks/useForgotPassword";
import {
  forgotPasswordSchema,
  ForgotPasswordForm,
} from "@/features/auth/schemas/forgotPassword.schema";
import { getProblemDetails } from "@/lib/api/errors";

export default function ForgotPasswordPage() {
  const forgotPasswordMutation = useForgotPassword();

  const { control, handleSubmit } = useForm<ForgotPasswordForm>({
    resolver: zodResolver(forgotPasswordSchema),
    defaultValues: {
      email: "",
    },
  });

  async function onSubmit(data: ForgotPasswordForm) {
    await forgotPasswordMutation.mutateAsync(data);
  }

  const problemDetails = getProblemDetails(forgotPasswordMutation.error);

  const error = forgotPasswordMutation.isError
    ? (problemDetails?.detail ?? "Unable to process your request.")
    : null;

  return (
    <AuthLayout>
      <div className="rounded-2xl border border-slate-200 bg-white p-6 shadow-sm sm:p-8">
        {!forgotPasswordMutation.isSuccess ? (
          <>
            <AuthHeader
              title="Forgot your password?"
              description="Enter your email address and we'll send you instructions to reset your password."
            />

            <form
              noValidate
              onSubmit={handleSubmit(onSubmit)}
              className="space-y-5"
            >
              <div>
                <label
                  htmlFor="email"
                  className="mb-2 block text-sm font-medium text-slate-700"
                >
                  Email address
                </label>

                <FormInput
                  control={control}
                  name="email"
                  type="email"
                  placeholder="you@company.com"
                />
              </div>

              <AuthError message={error} />

              <Button
                className="w-full"
                htmlType="submit"
                isLoading={forgotPasswordMutation.isPending}
              >
                Request Password Reset
              </Button>
            </form>

            <p className="mt-6 text-center text-sm text-slate-600">
              Remember your password?{" "}
              <Link
                href="/login"
                className="font-semibold text-blue-600 transition-colors hover:text-blue-700"
              >
                Sign in
              </Link>
            </p>
          </>
        ) : (
          <div className="text-center">
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
              Check your email
            </h1>

            <p className="mt-3 text-sm leading-6 text-slate-500">
              If an account exists for that email address, you&apos;ll receive
              instructions to reset your password.
            </p>

            <Link
              href="/login"
              className="mt-6 inline-flex font-semibold text-blue-600 transition-colors hover:text-blue-700"
            >
              Back to sign in
            </Link>
          </div>
        )}
      </div>
    </AuthLayout>
  );
}
