import { AxiosError } from "axios";
import { ProblemDetails } from "./types";

export function getProblemDetails(error: unknown): ProblemDetails | null {
  if (!(error instanceof AxiosError)) {
    return null;
  }

  return (error.response?.data as ProblemDetails) ?? null;
}
