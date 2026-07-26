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

interface TicketActionsProps {
  ticket: TicketListItem;
}

export default function TicketActions({ ticket }: TicketActionsProps) {
  const getActions = (): MenuProps["items"] => {
    const actions: MenuProps["items"] = [
      {
        key: "details",
        label: <Link href={`/tickets/${ticket.id}`}>View details</Link>,
      },
    ];

    switch (ticket.status) {
      case "Open":
        actions.push(
          {
            key: "assign",
            label: (
              <AssignTicketAction
                ticketId={ticket.id}
                ticketSubject={ticket.subject}
              />
            ),
          },
          {
            key: "close",
            label: <CloseTicketAction ticketId={ticket.id} />,
          },
        );
        break;

      case "Assigned":
        actions.push(
          {
            key: "start",
            label: "Start progress",
          },
          {
            key: "unassign",
            label: "Unassign ticket",
          },
          {
            key: "close",
            label: <CloseTicketAction ticketId={ticket.id} />,
          },
        );
        break;

      case "InProgress":
        actions.push(
          {
            key: "resolve",
            label: "Resolve ticket",
          },
          {
            key: "close",
            label: <CloseTicketAction ticketId={ticket.id} />,
          },
        );
        break;

      case "Resolved":
        actions.push(
          {
            key: "reopen",
            label: <ReopenTicketAction ticketId={ticket.id} />,
          },
          {
            key: "close",
            label: <CloseTicketAction ticketId={ticket.id} />,
          },
        );
        break;

      case "Reopened":
        actions.push(
          {
            key: "assign",
            label: "Assign ticket",
          },
          {
            key: "start",
            label: "Start progress",
          },
          {
            key: "close",
            label: <CloseTicketAction ticketId={ticket.id} />,
          },
        );
        break;

      case "Closed":
        actions.push(
          {
            key: "reopen",
            label: <ReopenTicketAction ticketId={ticket.id} />,
          },
          {
            key: "history",
            label: "View history",
          },
        );
        break;
    }
    if (ticket.status !== "Closed") {
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
