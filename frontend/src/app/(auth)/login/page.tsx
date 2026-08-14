"use client";

import Link from "next/link";
import { useRouter } from "next/navigation";
import { AxiosError } from "axios";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";

import { useLogin } from "@/features/auth/hooks/useLogin";
import { LoginForm, loginSchema } from "@/features/auth/schemas/login.schema";

import Button from "@/components/ui/Button";
import FormInput from "@/components/form/FormInput";
import FormPasswordInput from "@/components/form/FormPasswordInput";

import api from "@/lib/axios";
import AuthLayout from "@/features/auth/components/AuthLayout";
import AuthHeader from "@/features/auth/components/AuthHeader";
import AuthError from "@/features/auth/components/AuthError";

export default function LoginPage() {
  const router = useRouter();
  const loginMutation = useLogin();

  const { control, handleSubmit } = useForm<LoginForm>({
    resolver: zodResolver(loginSchema),
    defaultValues: {
      email: "",
      password: "",
    },
  });

  async function onSubmit(data: LoginForm) {
    try {
      await loginMutation.mutateAsync(data);

      await api.get("/auth/csrf");

      router.push("/dashboard");
    } catch {
      // Error is exposed through loginMutation.error.
    }
  }

  const error =
    loginMutation.error instanceof AxiosError
      ? (loginMutation.error.response?.data?.message ??
        "Unable to sign in. Please check your credentials.")
      : loginMutation.isError
        ? "Unable to sign in. Please check your credentials."
        : null;

  return (
    <AuthLayout>
      <div className="rounded-2xl border border-slate-200 bg-white p-6 shadow-sm sm:p-8">
        <AuthHeader
          title="Welcome back"
          description="Sign in to your SupportFlow workspace to continue."
        />

        <form
          noValidate
          onSubmit={handleSubmit(onSubmit)}
          className="space-y-5"
        >
          <FormInput
            control={control}
            name="email"
            type="email"
            placeholder="you@company.com"
          />

          <div>
            <div className="mb-2 flex items-center justify-between">
              <label
                htmlFor="password"
                className="text-sm font-medium text-slate-700"
              >
                Password
              </label>

              <Link
                href="/forgot-password"
                className="text-sm font-medium text-blue-600 transition-colors hover:text-blue-700"
              >
                Forgot password?
              </Link>
            </div>

            <FormPasswordInput
              control={control}
              name="password"
              placeholder="Enter your password"
            />
          </div>

          <AuthError message={error} />

          <Button
            className="w-full"
            htmlType="submit"
            isLoading={loginMutation.isPending}
          >
            Sign In
          </Button>
        </form>

        <div className="my-6 flex items-center gap-4">
          <div className="h-px flex-1 bg-slate-200" />
          <span className="text-xs text-slate-400">OR</span>
          <div className="h-px flex-1 bg-slate-200" />
        </div>

        <p className="text-center text-sm text-slate-600">
          Don&apos;t have a company account?{" "}
          <Link
            href="/register"
            className="font-semibold text-blue-600 transition-colors hover:text-blue-700"
          >
            Create one
          </Link>
        </p>
      </div>

      <p className="mt-6 text-center text-xs leading-5 text-slate-500">
        By continuing, you agree to SupportFlow&apos;s terms and privacy policy.
      </p>
    </AuthLayout>
  );
}
