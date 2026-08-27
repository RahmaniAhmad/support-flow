import { z } from "zod";
import { USER_VALIDATION } from "../constants/user-validation";

export const resetPasswordSchema = z.object({
  password: z
    .string()
    .min(
      USER_VALIDATION.PASSWORD_MIN_LENGTH,
      `Password must be at least ${USER_VALIDATION.PASSWORD_MIN_LENGTH} characters.`,
    ),
});

export type ResetPasswordFormData = z.infer<typeof resetPasswordSchema>;
