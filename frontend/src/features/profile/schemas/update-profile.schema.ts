import { z } from "zod";

export const updateProfileSchema = z.object({
  firstName: z.string().min(2, "First name is required."),

  lastName: z.string().min(2, "Last name is required."),

  phone: z.string().optional(),
});

export type UpdateProfileFormData = z.infer<typeof updateProfileSchema>;
