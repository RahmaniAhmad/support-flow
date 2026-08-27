import { z } from "zod";
import { USER_VALIDATION } from "../constants/user-validation";

export const updateUserSchema = z.object({
  firstName: z
    .string()
    .min(1, "First name is required.")
    .max(
      USER_VALIDATION.FIRST_NAME_MAX_LENGTH,
      `First name must be ${USER_VALIDATION.FIRST_NAME_MAX_LENGTH} characters or fewer.`,
    ),

  lastName: z
    .string()
    .min(1, "Last name is required.")
    .max(
      USER_VALIDATION.LAST_NAME_MAX_LENGTH,
      `Last name must be ${USER_VALIDATION.LAST_NAME_MAX_LENGTH} characters or fewer.`,
    ),

  phone: z
    .string()
    .max(
      USER_VALIDATION.PHONE_MAX_LENGTH,
      `Phone number must be ${USER_VALIDATION.PHONE_MAX_LENGTH} characters or fewer.`,
    )
    .optional(),
});

export type UpdateUserFormData = z.infer<typeof updateUserSchema>;
