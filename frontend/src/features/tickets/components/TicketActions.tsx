"use client";

import { Button, Dropdown, message, Modal } from "antd";
import type { MenuProps } from "antd";

import { useCallback, useMemo } from "react";

import { TicketListItem } from "../types";

import { useCurrentUser } from "@/features/auth/providers/CurrentUserProvider";
import { Permission } from "@/features/auth/Permissions";
import { hasPermission } from "@/features/auth/authorization";

import { getTicketActions } from "../utils/getTicketActions";

import { useStartProgressTicket } from "../hooks/useStartProgressTicket";
import { useMoveTicketToPending } from "../hooks/useMoveTicketToPending";
import { useResolveTicket } from "../hooks/useResolveTicket";
import { useCloseTicket } from "../hooks/useCloseTicket";
import { useReopenTicket } from "../hooks/useReopenTicket";
import { TicketAction, TicketActionKeys } from "../constants/ticketActions";
import { EllipsisVertical } from "lucide-react";

interface TicketActionsProps {
  ticket: TicketListItem;
}

export default function TicketActions({ ticket }: TicketActionsProps) {
  const currentUser = useCurrentUser();

  const startProgressMutation = useStartProgressTicket();
  const moveToPendingMutation = useMoveTicketToPending();
  const resolveMutation = useResolveTicket();
  const closeMutation = useCloseTicket();
  const reopenMutation = useReopenTicket();

  const can = useCallback(
    (permission: Permission) => hasPermission(currentUser, permission),
    [currentUser],
  );

  const items = useMemo(
    () =>
      getTicketActions({
        ticket,
        can,
      }),
    [ticket, can],
  );

  const handleConfirmAction = useCallback(
    (
      title: string,
      content: string,
      action: () => Promise<void>,
      danger = false,
    ) => {
      Modal.confirm({
        title,
        content,

        okText: "Confirm",
        cancelText: "Cancel",

        okButtonProps: {
          danger,
        },

        centered: true,

        onOk: action,
      });
    },
    [],
  );

  const handleClose = useCallback(() => {
    handleConfirmAction(
      "Close ticket",
      "Are you sure you want to close this ticket? Closed tickets cannot receive new comments.",

      () =>
        new Promise<void>((resolve, reject) => {
          closeMutation.mutate(ticket.id, {
            onSuccess: () => {
              message.success("Ticket closed successfully.");
              resolve();
            },

            onError: () => {
              message.error("Failed to close ticket.");
              reject();
            },
          });
        }),

      true,
    );
  }, [ticket.id, closeMutation, handleConfirmAction]);

  const handleReopen = useCallback(() => {
    handleConfirmAction(
      "Reopen ticket",
      "Are you sure you want to reopen this ticket?",

      () =>
        new Promise<void>((resolve, reject) => {
          reopenMutation.mutate(ticket.id, {
            onSuccess: () => {
              message.success("Ticket reopened successfully.");
              resolve();
            },

            onError: () => {
              message.error("Failed to reopen ticket.");
              reject();
            },
          });
        }),
    );
  }, [ticket.id, reopenMutation, handleConfirmAction]);

  const handleAction: MenuProps["onClick"] = useCallback(
    ({ key }) => {
      switch (key as TicketAction) {
        case TicketActionKeys.StartProgress:
          startProgressMutation.mutate(ticket.id, {
            onSuccess: () => message.success("Ticket started."),

            onError: () => message.error("Failed to start ticket."),
          });
          break;

        case TicketActionKeys.MoveToPending:
          moveToPendingMutation.mutate(ticket.id, {
            onSuccess: () => message.success("Ticket moved to pending."),

            onError: () => message.error("Failed to move ticket to pending."),
          });
          break;

        case TicketActionKeys.Resolve:
          resolveMutation.mutate(ticket.id, {
            onSuccess: () => message.success("Ticket resolved."),

            onError: () => message.error("Failed to resolve ticket."),
          });
          break;

        case TicketActionKeys.Close:
          handleClose();
          break;

        case TicketActionKeys.Reopen:
          handleReopen();
          break;
      }
    },
    [
      ticket.id,
      startProgressMutation,
      moveToPendingMutation,
      resolveMutation,
      handleClose,
      handleReopen,
    ],
  );

  return (
    <Dropdown
      trigger={["click"]}
      menu={{
        items,
        onClick: handleAction,
      }}
    >
      <Button type="text" icon={<EllipsisVertical size={16} />} />
    </Dropdown>
  );
}
