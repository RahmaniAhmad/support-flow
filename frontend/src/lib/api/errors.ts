import axios from "axios";

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
