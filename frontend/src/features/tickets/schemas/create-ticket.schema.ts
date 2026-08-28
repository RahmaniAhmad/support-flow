import { z } from "zod";
import { TICKET_VALIDATION } from "../constants/ticket-validation";

export const createTicketSchema = z.object({
  subject: z
    .string()
    .min(1, "Subject is required.")
    .max(
      TICKET_VALIDATION.SUBJECT_MAX_LENGTH,
      `Subject must be ${TICKET_VALIDATION.SUBJECT_MAX_LENGTH} characters or fewer.`,
    ),
  description: z
    .string()
    .min(1, "Description is required.")
    .max(
      TICKET_VALIDATION.DESCRIPTION_MAX_LENGTH,
      `Description must be ${TICKET_VALIDATION.DESCRIPTION_MAX_LENGTH} characters or fewer.`,
    ),
});

export type CreateTicketFormData = z.infer<typeof createTicketSchema>;
