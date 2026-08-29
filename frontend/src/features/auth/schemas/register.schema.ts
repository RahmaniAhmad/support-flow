import { z } from "zod";
import { AUTH_VALIDATION } from "../constants/auth-validation";

export const registerSchema = z
  .object({
    companyName: z
      .string()
      .trim()
      .min(1, "Company name is required")
      .max(
        AUTH_VALIDATION.COMPANY_NAME_MAX_LENGTH,
        `Company name must be ${AUTH_VALIDATION.COMPANY_NAME_MAX_LENGTH} characters or fewer.`,
      ),
    email: z
      .email("Please enter a valid email address")
      .trim()
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

    confirmPassword: z.string(),
  })
  .refine((x) => x.password === x.confirmPassword, {
    path: ["confirmPassword"],
    message: "Passwords do not match",
  });

export type RegisterFormData = z.infer<typeof registerSchema>;
