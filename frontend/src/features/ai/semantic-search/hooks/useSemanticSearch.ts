import { useMutation } from "@tanstack/react-query";
import { semanticSearch } from "../api";

export function useSemanticSearch() {
  return useMutation({
    mutationFn: ({ query, limit }: { query: string; limit?: number }) =>
      semanticSearch(query, limit),
  });
}
