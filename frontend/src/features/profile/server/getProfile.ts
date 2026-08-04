import "server-only";

import serverApi from "@/lib/server-api";

export async function getProfile() {
  try {
    const response = await serverApi.get("/users/profile");

    return response.data;
  } catch {
    return null;
  }
}
