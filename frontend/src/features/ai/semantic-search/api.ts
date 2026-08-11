import api from "@/lib/axios";
import { SemanticSearchResponse } from "./type";

export async function semanticSearch(query: string, limit = 5) {
  const response = await api.post<SemanticSearchResponse>(
    "/ai/semantic-search",
    {
      query,
      limit,
    },
  );

  return response.data;
}
