import { z } from "zod";
import { KNOWLEDGE_ARTICLES_VALIDATION } from "../constants/knowledge-articles-validation";

export const createArticleSchema = z.object({
  title: z
    .string()
    .min(1, "Title is required")
    .max(
      KNOWLEDGE_ARTICLES_VALIDATION.TITLE_MAX_LENGTH,
      `Title must be ${KNOWLEDGE_ARTICLES_VALIDATION.TITLE_MAX_LENGTH} characters or fewer.`,
    ),
  content: z
    .string()
    .min(1, "Content is required")
    .max(
      KNOWLEDGE_ARTICLES_VALIDATION.CONTENT_MAX_LENGTH,
      `Content must be ${KNOWLEDGE_ARTICLES_VALIDATION.CONTENT_MAX_LENGTH} characters or fewer.`,
    ),
});

export type CreateArticleFormData = z.infer<typeof createArticleSchema>;
