"use client";

import { WidgetSkeleton } from "@/components/ui/skeleton";
import { useAgentPerformance } from "../hooks/useAgentPerformance";

export default function AgentPerformance() {
  const { data, isPending, error } = useAgentPerformance();

  if (isPending) return <WidgetSkeleton />;

  if (error || !data) {
    return (
      <div className="rounded-xl border bg-white p-6">
        Failed to load agents
      </div>
    );
  }

  return (
    <div className="rounded-xl border border-gray-300 bg-white p-6">
      <h3 className="mb-4 font-semibold">Agent Performance</h3>

      <div className="space-y-4">
        {data.map((agent) => (
          <div
            key={agent.userId}
            className="
              flex
              items-center
              justify-between
              border-b
              pb-3
              last:border-0
              "
          >
            <div>
              <p className="font-medium">{agent.name}</p>

              <p className="text-sm text-gray-500">
                {agent.assignedTickets} assigned
              </p>
            </div>

            <div className="text-right">
              <p className="font-semibold">{agent.resolvedTickets}</p>

              <p className="text-xs text-gray-500">resolved</p>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}
