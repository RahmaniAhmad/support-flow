"use client";

import { logout } from "@/features/auth/services/auth.service";
import { useRouter } from "next/navigation";

export default function Navbar() {
  const router = useRouter();

  const handleLogout = async () => {
    await logout();

    router.replace("/login");
  };

  return (
    <header className="flex h-16 items-center justify-between border-b bg-white px-6">
      <h1 className="font-semibold">Ticket Management System</h1>

      <button
        onClick={handleLogout}
        className="rounded bg-red-600 px-4 py-2 text-white"
      >
        Sign Out
      </button>
    </header>
  );
}
