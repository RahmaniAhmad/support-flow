import { z } from "zod";

export const createTicketSchema = z.object({
  subject: z
    .string()
    .min(3, "Subject must be at least 3 characters.")
    .max(200, "Subject cannot exceed 200 characters."),
  description: z
    .string()
    .min(10, "Description must be at least 10 characters.")
    .max(5000, "Description cannot exceed 5000 characters."),
});

export type CreateTicketFormData = z.infer<typeof createTicketSchema>;
