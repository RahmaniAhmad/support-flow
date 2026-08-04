"use client";

import { useRouter } from "next/navigation";
import Button from "@/components/ui/Button";

export default function NotFound() {
  const router = useRouter();

  return (
    <div className="flex min-h-[60vh] items-center justify-center px-6">
      <div className="max-w-md text-center">
        <p className="text-6xl font-bold text-slate-300">404</p>

        <h1 className="mt-4 text-2xl font-semibold text-slate-900">
          Page not found
        </h1>

        <p className="mt-3 text-sm text-slate-600">
          The resource you are looking for does not exist or may have been
          removed.
        </p>

        <div className="mt-6 flex justify-center gap-3">
          <Button onClick={() => router.push("/dashboard")}>
            Go to dashboard
          </Button>

          <Button type="default" onClick={() => router.back()}>
            Go back
          </Button>
        </div>
      </div>
    </div>
  );
}
