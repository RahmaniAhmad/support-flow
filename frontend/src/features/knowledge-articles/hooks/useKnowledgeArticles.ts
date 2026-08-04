import { useQuery } from "@tanstack/react-query";
import { queryKeys } from "@/lib/react-query/queryKeys";
import { getKnowledgeArticles } from "../api/knowledgeArticles";

export function useKnowledgeArticles() {
  return useQuery({
    queryKey: queryKeys.knowledgeArticles.list(),
    queryFn: getKnowledgeArticles,
  });
}
