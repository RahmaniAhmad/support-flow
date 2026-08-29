"use client";

import Link from "next/link";
import { useRouter } from "next/navigation";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";

import { useRegister } from "@/features/auth/hooks/useRegister";
import {
  RegisterFormData,
  registerSchema,
} from "@/features/auth/schemas/register.schema";

import Button from "@/components/ui/Button";
import FormInput from "@/components/form/FormInput";
import FormPasswordInput from "@/components/form/FormPasswordInput";

import AuthLayout from "@/features/auth/components/AuthLayout";
import AuthHeader from "@/features/auth/components/AuthHeader";

import FormLabel from "@/components/form/FormLabel";
import { AUTH_VALIDATION } from "../constants/auth-validation";
import FormError from "@/components/form/FormError";

export default function RegisterForm() {
  const router = useRouter();

  const registerMutation = useRegister();

  const { control, handleSubmit } = useForm<RegisterFormData>({
    resolver: zodResolver(registerSchema),
    defaultValues: {
      companyName: "",
      email: "",
      password: "",
      confirmPassword: "",
    },
  });

  function onSubmit(data: RegisterFormData) {
    registerMutation.mutate(
      {
        companyName: data.companyName,
        email: data.email,
        password: data.password,
      },
      {
        onSuccess: () => {
          router.push("/login");
        },
      },
    );
  }

  return (
    <AuthLayout>
      <div className="rounded-2xl border border-slate-200 bg-white p-6 shadow-sm sm:p-8">
        <AuthHeader
          title="Create your workspace"
          description="Set up your company workspace and start managing customer support."
        />

        <form
          noValidate
          onSubmit={handleSubmit(onSubmit)}
          className="space-y-5"
        >
          <div>
            <FormLabel>Company name</FormLabel>
            <FormInput
              control={control}
              name="companyName"
              type="text"
              placeholder="Acme Inc."
              maxLength={AUTH_VALIDATION.COMPANY_NAME_MAX_LENGTH}
            />
          </div>

          <div>
            <FormLabel>Administrator email</FormLabel>
            <FormInput
              control={control}
              name="email"
              type="email"
              placeholder="admin@company.com"
              maxLength={AUTH_VALIDATION.EMAIL_MAX_LENGTH}
            />
          </div>

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

          {registerMutation.error && (
            <FormError error={registerMutation.error} />
          )}

          <Button
            className="w-full"
            htmlType="submit"
            isLoading={registerMutation.isPending}
          >
            Create Workspace
          </Button>
        </form>

        <div className="my-6 flex items-center gap-4">
          <div className="h-px flex-1 bg-slate-200" />
          <span className="text-xs text-slate-400">ALREADY A MEMBER?</span>
          <div className="h-px flex-1 bg-slate-200" />
        </div>

        <p className="text-center text-sm text-slate-600">
          Already have a SupportFlow account?{" "}
          <Link
            href="/login"
            className="font-semibold text-blue-600 transition-colors hover:text-blue-700"
          >
            Sign in
          </Link>
        </p>
      </div>

      <p className="mt-6 text-center text-xs leading-5 text-slate-500">
        By creating an account, you agree to SupportFlow&apos;s terms and
        privacy policy.
      </p>
    </AuthLayout>
  );
}
