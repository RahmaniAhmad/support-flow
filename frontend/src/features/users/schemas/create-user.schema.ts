import { z } from "zod";
import { USER_VALIDATION } from "../constants/user-validation";

export const createUserSchema = z.object({
  firstName: z
    .string()
    .trim()
    .min(1, "First name is required.")
    .max(
      USER_VALIDATION.FIRST_NAME_MAX_LENGTH,
      `First name must be ${USER_VALIDATION.FIRST_NAME_MAX_LENGTH} characters or fewer.`,
    ),

  lastName: z
    .string()
    .trim()
    .min(1, "Last name is required.")
    .max(
      USER_VALIDATION.LAST_NAME_MAX_LENGTH,
      `Last name must be ${USER_VALIDATION.LAST_NAME_MAX_LENGTH} characters or fewer.`,
    ),

  email: z
    .email("Invalid email address.")
    .max(
      USER_VALIDATION.EMAIL_MAX_LENGTH,
      `Email must be ${USER_VALIDATION.EMAIL_MAX_LENGTH} characters or fewer.`,
    ),

  password: z
    .string()
    .min(
      USER_VALIDATION.PASSWORD_MIN_LENGTH,
      `Password must be at least ${USER_VALIDATION.PASSWORD_MIN_LENGTH} characters.`,
    ),

  phone: z
    .string()
    .trim()
    .max(
      USER_VALIDATION.PHONE_MAX_LENGTH,
      `Phone number must be ${USER_VALIDATION.PHONE_MAX_LENGTH} characters or fewer.`,
    )
    .optional(),

  role: z.enum(["Agent", "Customer"]),
});

export type CreateUserFormData = z.infer<typeof createUserSchema>;
