"use client";

import StatCard from "@/features/dashboard/components/StatCard";
import { useDashboard } from "@/features/dashboard/hooks/useDashboard";

export default function DashboardPage() {
  const { data, isPending, error } = useDashboard();

  if (isPending) {
    return <div>Loading...</div>;
  }

  if (error || !data) {
    return <div>Failed to load dashboard</div>;
  }

  return (
    <div>
      <h1 className="mb-6 text-3xl font-bold">Dashboard</h1>

      <div className="grid gap-6 md:grid-cols-2 xl:grid-cols-5">
        <StatCard title="Open" value={data.openTickets} />

        <StatCard title="In Progress" value={data.inProgressTickets} />

        <StatCard title="Resolved" value={data.resolvedTickets} />

        <StatCard title="Closed" value={data.closedTickets} />

        <StatCard title="Unassigned" value={data.unassignedTickets} />
      </div>
    </div>
  );
}
