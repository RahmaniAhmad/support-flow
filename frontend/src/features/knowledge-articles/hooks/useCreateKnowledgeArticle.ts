import { useMutation, useQueryClient } from "@tanstack/react-query";
import { createKnowledgeArticle } from "../api/knowledgeArticles";
import { queryKeys } from "@/lib/react-query/queryKeys";

export function useCreateKnowledgeArticle() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: createKnowledgeArticle,
    onSuccess: () => {
      queryClient.invalidateQueries({
        queryKey: queryKeys.knowledgeArticles.all,
      });
    },
  });
}
