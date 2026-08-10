import api from "@/lib/axios";
import { SuggestedArticlesResponse } from "./type";

export async function getSuggestedArticles(ticketId: string) {
  const response = await api.get<SuggestedArticlesResponse>(
    `/tickets/${ticketId}/suggested-articles`,
  );

  return response.data;
}
