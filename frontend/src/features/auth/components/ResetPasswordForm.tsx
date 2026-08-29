"use client";

import Link from "next/link";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";

import AuthLayout from "@/features/auth/components/AuthLayout";
import AuthHeader from "@/features/auth/components/AuthHeader";

import Button from "@/components/ui/Button";

import FormLabel from "@/components/form/FormLabel";
import { AUTH_VALIDATION } from "../constants/auth-validation";
import FormError from "@/components/form/FormError";
import { useResetPassword } from "../hooks/useResetPassword";
import {
  ResetPasswordFormData,
  resetPasswordSchema,
} from "../schemas/resetPassword.schema";
import FormPasswordInput from "@/components/form/FormPasswordInput";
import { useRouter, useSearchParams } from "next/navigation";

export default function ResetPasswordForm() {
  const router = useRouter();
  const searchParams = useSearchParams();

  const token = searchParams.get("token");

  const resetPasswordMutation = useResetPassword();

  const { control, handleSubmit } = useForm<ResetPasswordFormData>({
    resolver: zodResolver(resetPasswordSchema),
    defaultValues: {
      password: "",
      confirmPassword: "",
    },
  });

  function onSubmit(data: ResetPasswordFormData) {
    if (!token) {
      return;
    }

    resetPasswordMutation.mutate(
      { token, password: data.password },
      { onSuccess: () => router.push("/login") },
    );
  }

  return (
    <AuthLayout>
      <div className="rounded-2xl border border-slate-200 bg-white p-6 shadow-sm sm:p-8">
        {!resetPasswordMutation.isSuccess ? (
          <>
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
                <FormLabel>Password</FormLabel>
                <FormPasswordInput
                  control={control}
                  name="password"
                  placeholder="Create a strong password"
                  maxLength={AUTH_VALIDATION.PASSWORD_MAX_LENGTH}
                />
              </div>

              <div>
                <FormLabel>Confirm password</FormLabel>
                <FormPasswordInput
                  control={control}
                  name="confirmPassword"
                  placeholder="Confirm your password"
                  maxLength={AUTH_VALIDATION.PASSWORD_MAX_LENGTH}
                />
              </div>

              {resetPasswordMutation.error && (
                <FormError error={resetPasswordMutation.error} />
              )}

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
          </>
        ) : (
          <div className="text-center">
            <h1 className="text-2xl font-bold text-slate-900">
              Check your email
            </h1>

            <p className="mt-3 text-sm leading-6 text-slate-500">
              If an account exists for that email address, you&apos;ll receive
              instructions to reset your password.
            </p>

            <Link
              href="/login"
              className="mt-6 inline-flex font-semibold text-blue-600 hover:text-blue-700"
            >
              Back to sign in
            </Link>
          </div>
        )}
      </div>
    </AuthLayout>
  );
}
