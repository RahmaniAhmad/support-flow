import "server-only";

import serverApi from "@/lib/server-api";

export async function getUser(id: string) {
  try {
    const response = await serverApi.get(`/users/${id}`);

    return response.data;
  } catch {
    return null;
  }
}
