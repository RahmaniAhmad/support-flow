import { z } from "zod";

export const createUserSchema = z.object({
  firstName: z.string().min(1, "First name is required."),

  lastName: z.string().min(1, "Last name is required."),

  email: z.string().email("Invalid email address."),

  password: z.string().min(8, "Password must be at least 8 characters."),

  phone: z.string().optional(),

  role: z.enum(["Agent", "Customer"]),
});

export type CreateUserFormData = z.infer<typeof createUserSchema>;
