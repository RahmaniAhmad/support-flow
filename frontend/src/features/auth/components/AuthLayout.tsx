import { Sparkles } from "lucide-react";
import Link from "next/link";
import { ReactNode } from "react";

interface AuthLayoutProps {
  children: ReactNode;
}

export default function AuthLayout({ children }: AuthLayoutProps) {
  return (
    <main className="min-h-screen bg-slate-950">
      <div className="grid min-h-screen lg:grid-cols-2">
        <section className="hidden flex-col justify-between bg-slate-950 p-10 text-white lg:flex">
          <div>
            <Link
              href="/login"
              className="inline-flex items-center gap-3 text-xl font-semibold"
            >
              <span className="flex h-9 w-9 items-center justify-center rounded-lg bg-blue-600 text-sm font-bold">
                S
              </span>
              SupportFlow
            </Link>
          </div>

          <div className="max-w-lg">
            <p className="flex items-center gap-2 mb-4 text-sm font-medium uppercase tracking-wider text-blue-400">
              <Sparkles strokeWidth={1.5} fill="currentColor" />
              Intelligent Customer Support
            </p>

            <h1 className="text-4xl font-bold leading-tight xl:text-5xl">
              Support your customers. Empower your team.
            </h1>

            <p className="mt-6 text-lg leading-8 text-slate-400">
              Manage tickets, knowledge, and customer support workflows with
              intelligent AI-powered assistance.
            </p>
          </div>

          <p className="text-sm text-slate-500">
            © {new Date().getFullYear()} SupportFlow. All rights reserved.
          </p>
        </section>

        <section className="flex min-h-screen items-center justify-center bg-slate-100 px-4 py-10 sm:px-6">
          <div className="w-full max-w-md">
            <div className="mb-8 text-center lg:hidden">
              <Link
                href="/login"
                className="inline-flex items-center gap-3 text-xl font-semibold text-slate-900"
              >
                <span className="flex h-9 w-9 items-center justify-center rounded-lg bg-blue-600 text-sm font-bold text-white">
                  S
                </span>
                SupportFlow
              </Link>
            </div>

            {children}
          </div>
        </section>
      </div>
    </main>
  );
}
