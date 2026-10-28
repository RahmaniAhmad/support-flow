import { z } from "zod";

export const ticketCategorySchema = z.object({
  name: z
    .string()
    .trim()
    .min(1, "Name is required.")
    .max(100, "Name must be 100 characters or fewer."),
  description: z
    .string()
    .max(500, "Description must be 500 characters or fewer.")
    .optional(),
});

export type TicketCategoryFormData = z.infer<typeof ticketCategorySchema>;
