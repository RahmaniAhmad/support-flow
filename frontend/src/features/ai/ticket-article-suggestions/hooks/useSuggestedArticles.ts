import { useQuery } from "@tanstack/react-query";
import { getSuggestedArticles } from "../api";

export function useSuggestedArticles(ticketId: string) {
  return useQuery({
    queryKey: ["ticket", ticketId, "suggested-articles"],

    queryFn: () => getSuggestedArticles(ticketId),

    enabled: !!ticketId,

    staleTime: 60000,
  });
}
