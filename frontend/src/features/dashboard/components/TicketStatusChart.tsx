"use client";

import {
  Bar,
  BarChart,
  CartesianGrid,
  ResponsiveContainer,
  Tooltip,
  XAxis,
  YAxis,
} from "recharts";
import { useDashboardStatistics } from "../hooks/useDashboardStatistics";

const STATUS_COLORS = {
  Open: "#3b82f6",
  Assigned: "#06b6d4",
  "In Progress": "#f97316",
  Pending: "#eab308",
  Resolved: "#22c55e",
  Reopened: "#a855f7",
  Closed: "#6b7280",
};

export default function TicketStatusChart() {
  const { data, isPending } = useDashboardStatistics();

  if (isPending) {
    return (
      <div className="rounded-xl border bg-card p-6">
        <div className="mb-6">
          <div className="h-5 w-32 animate-pulse rounded bg-muted" />
          <div className="mt-2 h-4 w-48 animate-pulse rounded bg-muted" />
        </div>

        <div className="h-75 animate-pulse rounded bg-muted/50" />
      </div>
    );
  }

  if (!data) {
    return null;
  }

  const chartData = [
    {
      name: "Open",
      value: data.openTickets,
      fill: STATUS_COLORS.Open,
    },
    {
      name: "Assigned",
      value: data.assignedTickets,
      fill: STATUS_COLORS.Assigned,
    },
    {
      name: "In Progress",
      value: data.inProgressTickets,
      fill: STATUS_COLORS["In Progress"],
    },
    {
      name: "Pending",
      value: data.pendingTickets,
      fill: STATUS_COLORS.Pending,
    },
    {
      name: "Resolved",
      value: data.resolvedTickets,
      fill: STATUS_COLORS.Resolved,
    },
    {
      name: "Reopened",
      value: data.reopenedTickets,
      fill: STATUS_COLORS.Reopened,
    },
    {
      name: "Closed",
      value: data.closedTickets,
      fill: STATUS_COLORS.Closed,
    },
  ];

  return (
    <div className="rounded-xl border border-gray-300 bg-white p-6">
      <div className="mb-6">
        <h2 className="text-lg font-semibold">Ticket Status</h2>
        <p className="text-sm text-muted-foreground">
          Current distribution of tickets by status
        </p>
      </div>

      <div className="h-75">
        <ResponsiveContainer width="100%" height="100%">
          <BarChart
            data={chartData}
            layout="vertical"
            margin={{
              top: 0,
              right: 16,
              left: 16,
              bottom: 0,
            }}
          >
            <CartesianGrid horizontal={false} strokeDasharray="3 3" />

            <XAxis
              type="number"
              allowDecimals={false}
              axisLine={false}
              tickLine={false}
            />

            <YAxis
              type="category"
              dataKey="name"
              width={85}
              axisLine={false}
              tickLine={false}
            />

            <Tooltip
              cursor={{ fill: "transparent" }}
              formatter={(value) => [value, "Tickets"]}
            />

            <Bar dataKey="value" radius={[0, 6, 6, 0]} barSize={24} />
          </BarChart>
        </ResponsiveContainer>
      </div>
    </div>
  );
}
