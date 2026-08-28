import { z } from "zod";
import { PROFILE_VALIDATION } from "../constants/profile-validation";

export const updateProfileSchema = z.object({
  firstName: z
    .string()
    .min(2, "First name is required.")
    .max(
      PROFILE_VALIDATION.FIRST_NAME_MAX_LENGTH,
      `First name must be ${PROFILE_VALIDATION.FIRST_NAME_MAX_LENGTH} characters or fewer.`,
    ),

  lastName: z
    .string()
    .min(2, "Last name is required.")
    .max(
      PROFILE_VALIDATION.LAST_NAME_MAX_LENGTH,
      `Last name must be ${PROFILE_VALIDATION.LAST_NAME_MAX_LENGTH} characters or fewer.`,
    ),

  phone: z
    .string()
    .max(
      PROFILE_VALIDATION.PHONE_MAX_LENGTH,
      `Phone number must be ${PROFILE_VALIDATION.PHONE_MAX_LENGTH} characters or fewer.`,
    )
    .optional(),
});

export type UpdateProfileFormData = z.infer<typeof updateProfileSchema>;
