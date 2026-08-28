import { useMutation, useQueryClient } from "@tanstack/react-query";
import { updateKnowledgeArticle } from "../api/knowledgeArticles";
import { queryKeys } from "@/lib/react-query/queryKeys";
import { UpdateKnowledgeArticleRequest } from "../types";

export function useUpdateKnowledgeArticle(id: string) {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (data: UpdateKnowledgeArticleRequest) =>
      updateKnowledgeArticle(id, data),

    onSuccess: () => {
      queryClient.invalidateQueries({
        queryKey: queryKeys.knowledgeArticles.all,
      });
    },
  });
}
