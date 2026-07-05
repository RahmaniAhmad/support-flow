import { z } from "zod";

export const registerSchema = z
  .object({
    companyName: z
      .string()
      .trim()
      .min(1, "Company name is required")
      .max(100, "Company name is too long"),

    email: z
      .string()
      .trim()
      .min(1, "Email is required")
      .pipe(z.email("Please enter a valid email address")),

    password: z.string().min(8, "Password must be at least 8 characters"),

    confirmPassword: z.string(),
  })
  .refine((x) => x.password === x.confirmPassword, {
    path: ["confirmPassword"],
    message: "Passwords do not match",
  });

export type RegisterForm = z.infer<typeof registerSchema>;
