"use client";

import PageContent from "@/components/ui/page/PageContent";
import PageHeader from "@/components/ui/page/PageHeader";
import PageTitle from "@/components/ui/page/PageTitle";

import DashboardStats from "@/features/dashboard/components/DashboardStats";

import TicketTrendChart from "@/features/dashboard/components/TicketTrendChart";

import AgentPerformance from "@/features/dashboard/components/AgentPerformance";

import RecentActivities from "@/features/dashboard/components/RecentActivities";
import TicketStatusChart from "@/features/dashboard/components/TicketStatusChart";

export default function DashboardPage() {
  return (
    <PageContent>
      <PageHeader>
        <PageTitle>Dashboard</PageTitle>
      </PageHeader>

      <div className="space-y-6">
        <DashboardStats />

        <div className="grid gap-6 xl:grid-cols-3">
          <div className="xl:col-span-2">
            <TicketTrendChart />
          </div>

          <TicketStatusChart />
        </div>

        <AgentPerformance />

        <RecentActivities />
      </div>
    </PageContent>
  );
}
