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
        <StatCard
          title="Open"
          value={data.openTickets}
          className="bg-blue-100 text-blue-600"
        />

        <StatCard
          title="In Progress"
          value={data.inProgressTickets}
          className="bg-orange-100 text-purple-600"
        />

        <StatCard
          title="Resolved"
          value={data.resolvedTickets}
          className="bg-green-100 text-green-600"
        />

        <StatCard
          title="Closed"
          value={data.closedTickets}
          className="bg-gray-100 text-gray-600"
        />

        <StatCard
          title="Unassigned"
          value={data.unassignedTickets}
          className="bg-red-100 text-amber-600"
        />
      </div>
    </div>
  );
}
