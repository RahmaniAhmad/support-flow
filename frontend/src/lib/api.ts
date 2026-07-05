const API_URL = process.env.NEXT_PUBLIC_API_URL;

export async function apiFetch<T>(
  path: string,
  options?: RequestInit & { token?: string },
): Promise<T> {
  const { token, headers, method, ...fetchOptions } = options || {};

  if (!API_URL) {
    throw new Error("NEXT_PUBLIC_API_URL is not defined");
  }

  const url = `${API_URL}${path}`;

  const response = await fetch(url, {
    ...fetchOptions,

    method: method ?? "GET",

    headers: {
      "Content-Type": "application/json",

      ...(token && { Authorization: `Bearer ${token}` }),

      ...(headers || {}),
    },
  });

  if (!response.ok) {
    const errorText = await response.text();
    throw new Error(`Error ${response.status}: ${errorText}`);
  }

  return response.json();
}
