import axios from "axios";
import type { FieldValues, Path, UseFormSetError } from "react-hook-form";

import type { ProblemDetails } from "./types";

export function getProblemDetails(error: unknown): ProblemDetails | null {
  if (!axios.isAxiosError(error)) {
    return null;
  }

  const data = error.response?.data;

  if (!data || typeof data !== "object") {
    return null;
  }

  return data as ProblemDetails;
}

export function getApiErrorMessage(
  error: unknown,
  fallbackMessage = "An unexpected error occurred.",
): string {
  const problemDetails = getProblemDetails(error);

  return problemDetails?.detail ?? fallbackMessage;
}

export function applyValidationErrors<TFieldValues extends FieldValues>(
  error: unknown,
  setError: UseFormSetError<TFieldValues>,
): string | null {
  const problemDetails = getProblemDetails(error);

  if (!problemDetails?.errors) {
    return problemDetails?.detail ?? null;
  }

  for (const [field, messages] of Object.entries(problemDetails.errors)) {
    if (!messages?.length) {
      continue;
    }

    const fieldName = field.charAt(0).toLowerCase() + field.slice(1);

    setError(fieldName as Path<TFieldValues>, {
      type: "server",
      message: messages.join(" "),
    });
  }

  return problemDetails.detail ?? null;
}
