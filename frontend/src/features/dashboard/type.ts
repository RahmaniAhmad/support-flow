export interface DashboardStatisticsRespons {
  totalTickets: number;
  openTickets: number;
  assignedTickets: number;
  inProgressTickets: number;
  pendingTickets: number;
  resolvedTickets: number;
  reopenedTickets: number;
  closedTickets: number;
  unassignedTickets: number;
}

export interface TicketTrendRespons {
  date: string;
  created: number;
  resolved: number;
}

export interface AgentPerformanceRespons {
  userId: string;
  name: string;
  assignedTickets: number;
  resolvedTickets: number;
}

export interface RecentActivityRespons {
  message: string;
  createdAtUtc: string;
}
