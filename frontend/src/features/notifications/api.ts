import api from "@/lib/api/axios";
import { AppNotification, UnreadCountResponse } from "./types";
import { PagedResponse } from "@/types/common";

export async function getNotifications(page = 1, pageSize = 20) {
  const response = await api.get<PagedResponse<AppNotification>>(
    "/notifications",
    {
      params: {
        page,
        pageSize,
      },
    },
  );

  return response.data;
}

export async function getUnreadCount() {
  const response = await api.get<UnreadCountResponse>(
    "/notifications/unread-count",
  );

  return response.data;
}

export async function markAsRead(id: string) {
  await api.put(`/notifications/${id}/read`);
}

export async function markAllAsRead() {
  await api.put("/notifications/read-all");
}
