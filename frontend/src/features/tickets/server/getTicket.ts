import serverApi from "@/lib/api/server-api";

export async function getTicket(id: string) {
  try {
    const response = await serverApi.get(`/tickets/${id}`);

    return response.data;
  } catch {
    return null;
  }
}
