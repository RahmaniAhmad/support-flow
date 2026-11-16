"use client";

import PageContent from "@/components/ui/page/PageContent";
import PageHeader from "@/components/ui/page/PageHeader";
import PageTitle from "@/components/ui/page/PageTitle";

import DashboardStats from "@/features/dashboard/components/DashboardStats";
import TicketTrendChart from "@/features/dashboard/components/TicketTrendChart";
import TicketStatusChart from "@/features/dashboard/components/TicketStatusChart";
import AgentPerformance from "@/features/dashboard/components/AgentPerformance";
import UnassignedTickets from "@/features/dashboard/components/UnassignedTickets";
import RecentActivities from "@/features/dashboard/components/RecentActivities";

export default function DashboardPage() {
  return (
    <PageContent>
      <PageHeader>
        <PageTitle>Dashboard</PageTitle>
      </PageHeader>

      <div className="space-y-6">
        {/* Overview */}
        <DashboardStats />

        {/* Analytics */}
        <div className="grid grid-cols-1 gap-6 xl:grid-cols-3">
          <div className="min-w-0 xl:col-span-2">
            <TicketTrendChart />
          </div>

          <div className="min-w-0">
            <TicketStatusChart />
          </div>
        </div>

        {/* Support Operations */}
        <div className="grid grid-cols-1 gap-6 xl:grid-cols-3">
          <div className="min-w-0 xl:col-span-2">
            <UnassignedTickets />
          </div>

          <div className="flex min-w-0 flex-col gap-6">
            <AgentPerformance />
            <RecentActivities />
          </div>
        </div>
      </div>
    </PageContent>
  );
}
