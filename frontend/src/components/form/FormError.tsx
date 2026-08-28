// "use client";

import { getProblemDetails } from "@/lib/api/errors";

type FormErrorProps = {
  error?: unknown;
};

export default function FormError({ error }: FormErrorProps) {
  if (!error) {
    return null;
  }

  const problemDetails = getProblemDetails(error);

  if (!problemDetails) {
    return (
      <div className="rounded-md border border-red-200 bg-red-50 px-3 py-2 text-sm text-red-700">
        Something went wrong. Please try again.
      </div>
    );
  }

  const validationErrors = problemDetails.errors
    ? Object.values(problemDetails.errors).flat()
    : [];

  return (
    <div className="rounded-md border border-red-200 bg-red-50 px-3 py-2 text-sm text-red-700">
      {problemDetails.detail && <p>{problemDetails.detail}</p>}

      {validationErrors.length > 0 && (
        <ul
          className={`list-disc space-y-1 pl-5 ${
            problemDetails.detail ? "mt-2" : ""
          }`}
        >
          {validationErrors.map((message, index) => (
            <li key={index}>{message}</li>
          ))}
        </ul>
      )}
    </div>
  );
}
