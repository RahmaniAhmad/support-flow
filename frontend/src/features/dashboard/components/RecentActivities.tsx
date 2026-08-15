"use client";

import { Clock } from "lucide-react";

import { useRecentActivities } from "../hooks/useRecentActivities";

export default function RecentActivities() {
  const { data, isPending, error } = useRecentActivities();

  if (isPending) {
    return (
      <div className="rounded-xl border bg-white p-6">
        Loading activities...
      </div>
    );
  }

  if (error || !data) {
    return (
      <div className="rounded-xl border bg-white p-6">
        Failed to load activities
      </div>
    );
  }

  return (
    <div className="rounded-xl border border-gray-300 bg-white p-6">
      <h3 className="mb-4 font-semibold">Recent Activity</h3>

      <div className="space-y-4">
        {data.map((activity, index) => (
          <div
            key={index}
            className="
            flex
            items-start
            gap-3
            "
          >
            <div
              className="
              rounded-full
              bg-gray-100
              p-2
            "
            >
              <Clock size={16} />
            </div>

            <div>
              <p className="text-sm">{activity.message}</p>

              <p className="text-xs text-gray-500">
                {new Date(activity.createdAtUtc).toLocaleString()}
              </p>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}
