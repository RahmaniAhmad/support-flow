import { z } from "zod";
import { AUTH_VALIDATION } from "../constants/auth-validation";

export const resetPasswordSchema = z
  .object({
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
    confirmPassword: z
      .string()
      .min(
        AUTH_VALIDATION.PASSWORD_MIN_LENGTH,
        `Password must be at least ${AUTH_VALIDATION.PASSWORD_MIN_LENGTH} characters`,
      )
      .max(
        AUTH_VALIDATION.PASSWORD_MAX_LENGTH,
        `Password must be ${AUTH_VALIDATION.PASSWORD_MAX_LENGTH} characters or fewer.`,
      ),
  })
  .refine((data) => data.password === data.confirmPassword, {
    message: "Passwords do not match.",
    path: ["confirmPassword"],
  });

export type ResetPasswordFormData = z.infer<typeof resetPasswordSchema>;
