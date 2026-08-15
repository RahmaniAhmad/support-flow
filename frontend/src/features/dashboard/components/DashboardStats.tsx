"use client";

import { Inbox, Clock3, CircleCheck, UserRoundX } from "lucide-react";

import StatCard from "./StatCard";

import { useDashboardStatistics } from "../hooks/useDashboardStatistics";

export default function DashboardStats() {
  const { data, isPending } = useDashboardStatistics();

  if (isPending) return <div>Loading...</div>;

  if (!data) return null;

  return (
    <div className="grid gap-6 md:grid-cols-2 xl:grid-cols-4">
      <StatCard
        title="Open"
        value={data.openTickets}
        icon={Inbox}
        className="bg-blue-100 text-blue-600"
      />

      <StatCard
        title="Pending"
        value={data.pendingTickets}
        icon={Clock3}
        className="bg-yellow-100 text-yellow-700"
      />

      <StatCard
        title="Resolved"
        value={data.resolvedTickets}
        icon={CircleCheck}
        className="bg-green-100 text-green-600"
      />

      <StatCard
        title="Unassigned"
        value={data.unassignedTickets}
        icon={UserRoundX}
        className="bg-red-100 text-red-600"
      />
    </div>
  );
}
