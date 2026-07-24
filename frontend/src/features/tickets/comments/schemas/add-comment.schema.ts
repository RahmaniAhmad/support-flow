import { z } from "zod";

export const addCommentSchema = z.object({
  content: z
    .string()
    .min(1, "Comment is required.")
    .max(4000, "Comment is too long."),
});

export type AddCommentFormData = z.infer<typeof addCommentSchema>;
