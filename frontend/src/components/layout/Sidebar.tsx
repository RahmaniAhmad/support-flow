"use client";

import Link from "next/link";

export default function Sidebar() {
  return (
    <aside className="w-64 border-r bg-white">
      <div className="border-b p-6 text-xl font-bold">VSA Tickets</div>

      <nav className="flex flex-col p-4">
        <Link href="/dashboard" className="rounded p-3 hover:bg-slate-100">
          Dashboard
        </Link>

        <Link href="/tickets" className="rounded p-3 hover:bg-slate-100">
          Tickets
        </Link>

        <Link href="/profile" className="rounded p-3 hover:bg-slate-100">
          Profile
        </Link>
      </nav>
    </aside>
  );
}
