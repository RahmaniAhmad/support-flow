import serverApi from "@/lib/server-api";

export async function getCurrentUser() {
  try {
    const response = await serverApi.get("/me");

    return response.data;
  } catch {
    return null;
  }
}
