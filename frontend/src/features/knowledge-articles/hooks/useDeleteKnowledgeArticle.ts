import { useMutation, useQueryClient } from "@tanstack/react-query";
import { deleteKnowledgeArticle } from "../api/knowledgeArticles";
import { queryKeys } from "@/lib/react-query/queryKeys";

export function useDeleteKnowledgeArticle() {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (id: string) => deleteKnowledgeArticle(id),
    onSuccess: () => {
      queryClient.invalidateQueries({
        queryKey: queryKeys.knowledgeArticles.all,
      });
    },
  });
}
