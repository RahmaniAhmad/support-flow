import { z } from "zod";

export const assignTicketSchema = z.object({
  assignedToUserId: z.string().min(1, "Please select a user."),
});

export type AssignTicketFormData = z.infer<typeof assignTicketSchema>;
