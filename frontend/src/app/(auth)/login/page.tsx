"use client";

import { useRouter } from "next/navigation";
import { useForm } from "react-hook-form";
import { AxiosError } from "axios";
import { useLogin } from "@/features/auth/hooks/useLogin";
import { LoginRequest } from "@/types/auth";
import { zodResolver } from "@hookform/resolvers/zod";
import { loginSchema } from "@/features/auth/schemas/login.schema";
import Button from "@/components/ui/Button";
import FormInput from "@/components/form/FormInput";
import FormPasswordInput from "@/components/form/FormPasswordInput";
import api from "@/lib/axios";

export default function LoginPage() {
  const router = useRouter();

  const loginMutation = useLogin();

  const { control, handleSubmit } = useForm<LoginRequest>({
    resolver: zodResolver(loginSchema),
  });

  async function onSubmit(request: LoginRequest) {
    try {
      await loginMutation.mutateAsync(request);
      await api.get("/auth/csrf");

      router.push("/dashboard");
    } catch (error) {
      console.error(error);
    }
  }

  const error =
    loginMutation.error instanceof AxiosError
      ? (loginMutation.error.response?.data?.message ?? "Invalid credentials")
      : loginMutation.isError
        ? "Invalid credentials"
        : null;

  return (
    <main className="flex min-h-screen items-center justify-center bg-slate-100">
      <form
        noValidate
        onSubmit={handleSubmit(onSubmit)}
        className="w-full max-w-md rounded-lg bg-white p-8 shadow"
      >
        <h1 className="mb-6 text-center text-3xl font-bold">Sign In</h1>

        <div className="flex flex-col mb-6 gap-y-4">
          <FormInput
            control={control}
            name="email"
            type="email"
            placeholder="Email"
          />

          <FormPasswordInput
            control={control}
            name="password"
            placeholder="Password"
          />
        </div>

        {error && <p className="mb-4 text-sm text-red-500">{error}</p>}

        <Button
          className="w-full"
          htmlType="submit"
          isLoading={loginMutation.isPending}
        >
          Sign In
        </Button>
        <div className="mt-6 text-center text-sm text-slate-600">
          Don&#39;t have a company account?{" "}
          <a
            href="/register"
            className="font-medium text-blue-600 transition-colors hover:text-blue-700 hover:underline"
          >
            Register
          </a>
        </div>
      </form>
    </main>
  );
}
