"use client";

import {
  LineChart,
  Line,
  XAxis,
  YAxis,
  Tooltip,
  ResponsiveContainer,
  CartesianGrid,
} from "recharts";

import { useTicketTrend } from "../hooks/useTicketTrend";

export default function TicketTrendChart() {
  const { data, isPending, error } = useTicketTrend("2026-08-01", "2026-08-31");

  if (isPending) {
    return (
      <div className="rounded-xl border bg-white p-6">Loading trend...</div>
    );
  }

  if (error || !data) {
    return (
      <div className="rounded-xl border bg-white p-6">Failed to load trend</div>
    );
  }

  return (
    <div className="rounded-xl border border-gray-300 bg-white p-6">
      <h3 className="mb-4 font-semibold">Ticket Trend</h3>

      <ResponsiveContainer width="100%" height={300}>
        <LineChart data={data}>
          <CartesianGrid />

          <XAxis dataKey="date" />

          <YAxis />

          <Tooltip />

          <Line
            type="monotone"
            dataKey="created"
            name="Created"
            strokeWidth={2}
          />

          <Line
            type="monotone"
            dataKey="resolved"
            name="Resolved"
            strokeWidth={2}
          />
        </LineChart>
      </ResponsiveContainer>
    </div>
  );
}
