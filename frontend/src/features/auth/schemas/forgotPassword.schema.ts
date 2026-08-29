import { z } from "zod";
import { AUTH_VALIDATION } from "../constants/auth-validation";

export const forgotPasswordSchema = z.object({
  email: z
    .email("Please enter a valid email address")
    .min(1, "Email is required")
    .max(
      AUTH_VALIDATION.EMAIL_MAX_LENGTH,
      `Email must be ${AUTH_VALIDATION.EMAIL_MAX_LENGTH} characters or fewer.`,
    ),
});

export type ForgotPasswordFormData = z.infer<typeof forgotPasswordSchema>;
