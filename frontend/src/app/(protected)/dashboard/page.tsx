"use client";

import PageContent from "@/components/ui/page/PageContent";
import PageHeader from "@/components/ui/page/PageHeader";
import PageTitle from "@/components/ui/page/PageTitle";
import StatCard from "@/features/dashboard/components/StatCard";
import { useDashboard } from "@/features/dashboard/hooks/useDashboard";
import {
  CircleCheck,
  CircleX,
  Clock3,
  Inbox,
  LoaderCircle,
  RotateCcw,
  UserCheck,
  UserRoundX,
} from "lucide-react";

export default function DashboardPage() {
  const { data, isPending, error } = useDashboard();

  if (isPending) {
    return <div>Loading...</div>;
  }

  if (error || !data) {
    return <div>Failed to load dashboard</div>;
  }

  return (
    <PageContent>
      <PageHeader>
        <PageTitle>Dashboard</PageTitle>
      </PageHeader>

      <div className="grid gap-6 md:grid-cols-2 xl:grid-cols-4">
        <StatCard
          title="Open"
          value={data.openTickets}
          icon={Inbox}
          className="bg-blue-100 text-blue-600"
        />

        <StatCard
          title="Assigned"
          value={data.assignedTickets}
          icon={UserCheck}
          className="bg-cyan-100 text-cyan-600"
        />

        <StatCard
          title="In Progress"
          value={data.inProgressTickets}
          icon={LoaderCircle}
          className="bg-orange-100 text-orange-600"
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
          title="Reopened"
          value={data.reopenedTickets}
          icon={RotateCcw}
          className="bg-purple-100 text-purple-600"
        />

        <StatCard
          title="Closed"
          value={data.closedTickets}
          icon={CircleX}
          className="bg-gray-100 text-gray-600"
        />

        <StatCard
          title="Unassigned"
          value={data.unassignedTickets}
          icon={UserRoundX}
          className="bg-red-100 text-red-600"
        />
      </div>
    </PageContent>
  );
}
