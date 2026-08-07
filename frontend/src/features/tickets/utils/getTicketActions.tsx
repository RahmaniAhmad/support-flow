import Link from "next/link";
import type { MenuProps } from "antd";

import { TicketListItem } from "../types";
import AssignTicketAction from "../assign/components/AssignTicketAction";
import {
  canAddComment,
  canAssignTicket,
  canCloseTicket,
  canMoveToPending,
  canReopenTicket,
  canResolveTicket,
  canStartProgress,
} from "../rules/ticketActionRules";

import { AppPermissions, Permission } from "@/features/auth/Permissions";
import AddCommentAction from "../comments/components/AddCommentAction";
import { TicketActionKeys } from "../constants/ticketActions";

type Params = {
  ticket: TicketListItem;
  can: (permission: Permission) => boolean;
};

export function getTicketActions({ ticket, can }: Params): MenuProps["items"] {
  const actions: MenuProps["items"] = [
    {
      key: TicketActionKeys.Details,
      label: <Link href={`/tickets/${ticket.id}`}>View details</Link>,
    },
  ];

  if (can(AppPermissions.TicketsAssign) && canAssignTicket(ticket)) {
    actions.push({
      key: TicketActionKeys.Assign,
      label: (
        <AssignTicketAction
          ticketId={ticket.id}
          ticketSubject={ticket.subject}
        />
      ),
    });
  }

  if (can(AppPermissions.TicketsStartProgress) && canStartProgress(ticket)) {
    actions.push({
      key: TicketActionKeys.StartProgress,
      label: "Start progress",
    });
  }

  if (can(AppPermissions.TicketsMoveToPending) && canMoveToPending(ticket)) {
    actions.push({
      key: TicketActionKeys.MoveToPending,
      label: "Move to pending",
    });
  }

  if (can(AppPermissions.TicketsResolve) && canResolveTicket(ticket)) {
    actions.push({
      key: TicketActionKeys.Resolve,
      label: "Resolve",
    });
  }

  if (can(AppPermissions.TicketsClose) && canCloseTicket(ticket)) {
    actions.push({
      key: TicketActionKeys.Close,
      label: "Close",
    });
  }

  if (can(AppPermissions.TicketsReopen) && canReopenTicket(ticket)) {
    actions.push({
      key: TicketActionKeys.Reopen,
      label: "Reopen",
    });
  }

  if (can(AppPermissions.TicketsComment) && canAddComment(ticket)) {
    actions.push(
      {
        type: "divider",
      },
      {
        key: TicketActionKeys.Comment,
        label: (
          <AddCommentAction
            ticketId={ticket.id}
            ticketSubject={ticket.subject}
          />
        ),
      },
    );
  }

  if (ticket.status === "Closed") {
    actions.push({
      key: TicketActionKeys.History,
      label: "View history",
    });
  }

  return actions;
}
