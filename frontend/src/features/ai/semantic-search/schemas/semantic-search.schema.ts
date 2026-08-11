import { z } from "zod";

export const semanticSearchSchema = z.object({
  query: z
    .string()
    .trim()
    .min(1, "Please enter a search query.")
    .max(500, "Search query is too long."),
});

export type SemanticSearchFormData = z.infer<typeof semanticSearchSchema>;
