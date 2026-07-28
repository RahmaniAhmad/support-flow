"use client";

import { Dropdown, Button } from "antd";
import { MoreOutlined } from "@ant-design/icons";
import type { MenuProps } from "antd";
import Link from "next/link";

import CloseTicketAction from "./CloseTicketAction";
import ReopenTicketAction from "./ReopenTicketAction";
import AddCommentButton from "../comments/components/AddCommentButton";
import AssignTicketAction from "../assign/components/AssignTicketAction";

import { TicketListItem } from "../types";

import { useCurrentUser } from "@/features/auth/providers/CurrentUserProvider";
import { AppPermissions, Permission } from "@/features/auth/Permissions";
import { hasPermission } from "@/features/auth/authorization";

interface TicketActionsProps {
  ticket: TicketListItem;
}

export default function TicketActions({ ticket }: TicketActionsProps) {
  const currentUser = useCurrentUser();

  const can = (permission: Permission) =>
    hasPermission(currentUser, permission);

  const getActions = (): MenuProps["items"] => {
    const actions: MenuProps["items"] = [
      {
        key: "details",
        label: <Link href={`/tickets/${ticket.id}`}>View details</Link>,
      },
    ];

    switch (ticket.status) {
      case "Open":
        if (can(AppPermissions.TicketsAssign)) {
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

        if (can(AppPermissions.TicketsClose)) {
          actions.push({
            key: "close",
            label: <CloseTicketAction ticketId={ticket.id} />,
          });
        }

        break;

      case "Assigned":
        if (can(AppPermissions.TicketsStartProgress)) {
          actions.push({
            key: "start",
            label: "Start progress",
          });
        }

        if (can(AppPermissions.TicketsUnassign)) {
          actions.push({
            key: "unassign",
            label: "Unassign ticket",
          });
        }

        if (can(AppPermissions.TicketsClose)) {
          actions.push({
            key: "close",
            label: <CloseTicketAction ticketId={ticket.id} />,
          });
        }

        break;

      case "InProgress":
        if (can(AppPermissions.TicketsResolve)) {
          actions.push({
            key: "resolve",
            label: "Resolve ticket",
          });
        }

        if (can(AppPermissions.TicketsClose)) {
          actions.push({
            key: "close",
            label: <CloseTicketAction ticketId={ticket.id} />,
          });
        }

        break;

      case "Resolved":
        if (can(AppPermissions.TicketsReopen)) {
          actions.push({
            key: "reopen",
            label: <ReopenTicketAction ticketId={ticket.id} />,
          });
        }

        if (can(AppPermissions.TicketsClose)) {
          actions.push({
            key: "close",
            label: <CloseTicketAction ticketId={ticket.id} />,
          });
        }

        break;

      case "Reopened":
        if (can(AppPermissions.TicketsAssign)) {
          actions.push({
            key: "assign",
            label: "Assign ticket",
          });
        }

        if (can(AppPermissions.TicketsStartProgress)) {
          actions.push({
            key: "start",
            label: "Start progress",
          });
        }

        if (can(AppPermissions.TicketsClose)) {
          actions.push({
            key: "close",
            label: <CloseTicketAction ticketId={ticket.id} />,
          });
        }

        break;

      case "Closed":
        if (can(AppPermissions.TicketsReopen)) {
          actions.push({
            key: "reopen",
            label: <ReopenTicketAction ticketId={ticket.id} />,
          });
        }

        actions.push({
          key: "history",
          label: "View history",
        });

        break;
    }

    if (ticket.status !== "Closed" && can(AppPermissions.TicketsComment)) {
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

    return actions;
  };

  const handleAction: MenuProps["onClick"] = ({ key }) => {
    console.log("action:", key, "ticket:", ticket.id);
  };

  return (
    <Dropdown
      menu={{
        items: getActions(),
        onClick: handleAction,
      }}
      trigger={["click"]}
    >
      <Button type="text" icon={<MoreOutlined />} />
    </Dropdown>
  );
}
