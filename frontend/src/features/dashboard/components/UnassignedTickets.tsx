"use client";

import Link from "next/link";
import { ArrowRight, Inbox, UserRoundX } from "lucide-react";

import { useUnassignedTickets } from "../hooks/useUnassignedTickets";

function formatDate(date: string) {
  return new Intl.DateTimeFormat("en", {
    month: "short",
    day: "numeric",
    hour: "2-digit",
    minute: "2-digit",
  }).format(new Date(date));
}

export default function UnassignedTickets() {
  const { data, isPending, error } = useUnassignedTickets();

  if (isPending) {
    return (
      <div className="flex h-full min-h-90 flex-col rounded-xl border border-gray-300 bg-white p-6">
        <div className="mb-6">
          <div className="h-5 w-40 animate-pulse rounded bg-gray-200" />

          <div className="mt-2 h-4 w-56 animate-pulse rounded bg-gray-200" />
        </div>

        <div className="space-y-4">
          {Array.from({ length: 5 }).map((_, index) => (
            <div
              key={index}
              className="flex items-center justify-between gap-4"
            >
              <div className="flex-1 space-y-2">
                <div className="h-4 w-3/4 animate-pulse rounded bg-gray-200" />

                <div className="h-3 w-1/3 animate-pulse rounded bg-gray-200" />
              </div>

              <div className="h-4 w-4 animate-pulse rounded bg-gray-200" />
            </div>
          ))}
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="flex h-full min-h-90 flex-col rounded-xl border border-gray-300 bg-white p-6">
        <div className="flex items-center gap-3">
          <div className="flex h-10 w-10 items-center justify-center rounded-lg bg-red-100 text-red-600">
            <UserRoundX className="h-5 w-5" />
          </div>

          <div>
            <h3 className="font-semibold">Unassigned Tickets</h3>

            <p className="text-sm text-muted-foreground">
              Failed to load tickets
            </p>
          </div>
        </div>
      </div>
    );
  }

  if (!data) {
    return null;
  }

  return (
    <div className="flex h-full min-h-90 flex-col rounded-xl border border-gray-300 bg-white p-6">
      {/* Header */}
      <div className="mb-5 flex items-start justify-between">
        <div>
          <h3 className="font-semibold">Unassigned Tickets</h3>

          <p className="mt-1 text-sm text-muted-foreground">
            Tickets waiting for an agent
          </p>
        </div>

        <div className="flex h-10 w-10 items-center justify-center rounded-lg bg-red-100 text-red-600">
          <UserRoundX className="h-5 w-5" />
        </div>
      </div>

      {/* Empty state */}
      {data.length === 0 ? (
        <div className="flex flex-1 flex-col items-center justify-center text-center">
          <div className="mb-3 flex h-12 w-12 items-center justify-center rounded-full bg-green-100 text-green-600">
            <Inbox className="h-6 w-6" />
          </div>

          <p className="font-medium">All tickets are assigned</p>

          <p className="mt-1 text-sm text-muted-foreground">
            There are no tickets waiting for an agent.
          </p>
        </div>
      ) : (
        <>
          {/* Tickets */}
          <div className="flex-1 divide-y">
            {data.map((ticket) => (
              <Link
                key={ticket.id}
                href={`/tickets/${ticket.id}`}
                className="flex items-center justify-between gap-4 py-3 transition-colors first:pt-0 last:pb-0 hover:bg-gray-50"
              >
                <div className="min-w-0 flex-1">
                  <div className="flex items-center gap-2">
                    <span className="shrink-0 text-sm font-medium">
                      #{ticket.ticketNumber}
                    </span>

                    <span className="truncate text-sm">{ticket.subject}</span>
                  </div>

                  <div className="mt-1 flex items-center gap-2 text-xs text-muted-foreground">
                    <span>{ticket.createdBy}</span>

                    <span>•</span>

                    <span>{formatDate(ticket.createdAtUtc)}</span>
                  </div>
                </div>

                <ArrowRight className="h-4 w-4 shrink-0 text-muted-foreground" />
              </Link>
            ))}
          </div>

          {/* Footer */}
          <div className="mt-5 border-t pt-4">
            <Link
              href="/tickets?status=unassigned"
              className="inline-flex items-center gap-2 text-sm font-medium text-primary hover:underline"
            >
              View all unassigned tickets
              <ArrowRight className="h-4 w-4" />
            </Link>
          </div>
        </>
      )}
    </div>
  );
}
