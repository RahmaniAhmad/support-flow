import { z } from "zod";
import { AUTH_VALIDATION } from "../constants/auth-validation";

export const loginSchema = z.object({
  email: z
    .email("Please enter a valid email address")
    .min(1, "Email is required")
    .max(
      AUTH_VALIDATION.EMAIL_MAX_LENGTH,
      `Email must be ${AUTH_VALIDATION.EMAIL_MAX_LENGTH} characters or fewer.`,
    ),
  password: z
    .string()
    .min(
      AUTH_VALIDATION.PASSWORD_MIN_LENGTH,
      `Password must be at least ${AUTH_VALIDATION.PASSWORD_MIN_LENGTH} characters`,
    )
    .max(
      AUTH_VALIDATION.PASSWORD_MAX_LENGTH,
      `Password must be ${AUTH_VALIDATION.PASSWORD_MAX_LENGTH} characters or fewer.`,
    ),
});

export type LoginFormData = z.infer<typeof loginSchema>;
