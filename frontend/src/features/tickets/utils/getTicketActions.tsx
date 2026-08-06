import Link from "next/link";
import type { MenuProps } from "antd";

import { TicketListItem } from "../types";

import AssignTicketAction from "../assign/components/AssignTicketAction";
import CloseTicketAction from "../components/CloseTicketAction";
import ReopenTicketAction from "../components/ReopenTicketAction";
import ResolveTicketAction from "../components/ResolveTicketAction";
import StartProgressAction from "../components/StartProgressAction";
import AddCommentButton from "../comments/components/AddCommentButton";

import {
  canAssignTicket,
  canCloseTicket,
  canReopenTicket,
  canAddComment,
  canResolveTicket,
  canStartProgress,
} from "../rules/ticketActionRules";

import { AppPermissions, Permission } from "@/features/auth/Permissions";

type Params = {
  ticket: TicketListItem;
  can: (permission: Permission) => boolean;
};

export function getTicketActions({ ticket, can }: Params): MenuProps["items"] {
  const actions: MenuProps["items"] = [];

  actions.push({
    key: "details",
    label: <Link href={`/tickets/${ticket.id}`}>View details</Link>,
  });

  if (can(AppPermissions.TicketsAssign) && canAssignTicket(ticket.status)) {
    actions.push({
      key: "assign",
      label: (
        <AssignTicketAction
          ticketId={ticket.id}
          ticketSubject={ticket.subject}
        />
      ),
    });
  }

  if (
    can(AppPermissions.TicketsStartProgress) &&
    canStartProgress(ticket.status)
  ) {
    actions.push({
      key: "start-progress",
      label: <StartProgressAction ticketId={ticket.id} />,
    });
  }

  if (can(AppPermissions.TicketsResolve) && canResolveTicket(ticket.status)) {
    actions.push({
      key: "resolve",
      label: <ResolveTicketAction ticketId={ticket.id} />,
    });
  }

  if (can(AppPermissions.TicketsClose) && canCloseTicket(ticket.status)) {
    actions.push({
      key: "close",
      label: <CloseTicketAction ticketId={ticket.id} />,
    });
  }

  if (can(AppPermissions.TicketsReopen) && canReopenTicket(ticket.status)) {
    actions.push({
      key: "reopen",
      label: <ReopenTicketAction ticketId={ticket.id} />,
    });
  }

  if (can(AppPermissions.TicketsComment) && canAddComment(ticket.status)) {
    actions.push(
      {
        type: "divider",
      },
      {
        key: "comment",
        label: (
          <AddCommentButton
            ticketId={ticket.id}
            ticketSubject={ticket.subject}
            variant="action"
          />
        ),
      },
    );
  }

  if (ticket.status === "Closed") {
    actions.push({
      key: "history",
      label: "View history",
    });
  }

  return actions;
}
