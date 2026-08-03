import { useMutation, useQueryClient } from "@tanstack/react-query";
import { updateKnowledgeArticle } from "../api/knowledgeArticles";
import { queryKeys } from "@/lib/react-query/queryKeys";

export function useUpdateKnowledgeArticle(id?: string) {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (data: unknown) =>
      updateKnowledgeArticle(id ?? "", data as any),
    onSuccess: () => {
      queryClient.invalidateQueries({
        queryKey: queryKeys.knowledgeArticles.all,
      });
    },
  });
}
