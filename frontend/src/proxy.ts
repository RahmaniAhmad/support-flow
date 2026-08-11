import { NextRequest, NextResponse } from "next/server";

const API_URL = process.env.API_URL!;
const REFRESH_BUFFER_MS = 30_000; // 30 seconds

function shouldRefreshToken(token: string): boolean {
  try {
    const [, payload] = token.split(".");

    if (!payload) {
      return true;
    }

    const decoded = JSON.parse(
      Buffer.from(payload, "base64url").toString("utf8"),
    );

    if (!decoded.exp) {
      return true;
    }

    // Refresh 30 seconds before expiration.
    return decoded.exp * 1000 <= Date.now() + REFRESH_BUFFER_MS;
  } catch {
    return true;
  }
}

function updateRequestCookies(
  request: NextRequest,
  setCookies: string[],
): Headers {
  const headers = new Headers(request.headers);

  const cookies = new Map<string, string>();

  const existingCookies = request.cookies.getAll();

  for (const cookie of existingCookies) {
    cookies.set(cookie.name, cookie.value);
  }

  for (const setCookie of setCookies) {
    const cookiePair = setCookie.split(";", 1)[0];

    const separatorIndex = cookiePair.indexOf("=");

    if (separatorIndex === -1) {
      continue;
    }

    const name = cookiePair.substring(0, separatorIndex);
    const value = cookiePair.substring(separatorIndex + 1);

    cookies.set(name, value);
  }

  headers.set(
    "Cookie",
    Array.from(cookies.entries())
      .map(([name, value]) => `${name}=${value}`)
      .join("; "),
  );

  return headers;
}

async function refreshTokens(request: NextRequest): Promise<{
  success: boolean;
  setCookies: string[];
}> {
  const refreshToken = request.cookies.get("refresh_token")?.value;

  if (!refreshToken) {
    return {
      success: false,
      setCookies: [],
    };
  }

  try {
    const refreshResponse = await fetch(`${API_URL}/auth/refresh`, {
      method: "POST",
      headers: {
        Cookie: `refresh_token=${refreshToken}`,
      },
      cache: "no-store",
    });

    if (!refreshResponse.ok) {
      console.error(
        "Token refresh failed:",
        refreshResponse.status,
        await refreshResponse.text(),
      );

      return {
        success: false,
        setCookies: [],
      };
    }

    const setCookies = refreshResponse.headers.getSetCookie();

    if (!setCookies.length) {
      console.error("Token refresh succeeded but no Set-Cookie headers.");

      return {
        success: false,
        setCookies: [],
      };
    }

    return {
      success: true,
      setCookies,
    };
  } catch (error) {
    console.error("Token refresh request failed:", error);

    return {
      success: false,
      setCookies: [],
    };
  }
}

export async function proxy(request: NextRequest) {
  const accessToken = request.cookies.get("access_token")?.value;
  const refreshToken = request.cookies.get("refresh_token")?.value;

  /*
   * No authentication cookies.
   */
  if (!accessToken && !refreshToken) {
    return NextResponse.redirect(new URL("/login", request.url));
  }

  /*
   * Access token exists and is still valid.
   */
  if (accessToken && !shouldRefreshToken(accessToken)) {
    return NextResponse.next();
  }

  /*
   * Access token is missing or expired.
   * Try to refresh using the refresh token.
   */
  if (refreshToken) {
    const refreshResult = await refreshTokens(request);

    if (refreshResult.success) {
      /*
       * IMPORTANT:
       *
       * Pass the NEW cookies to the downstream
       * Server Components for this same request.
       */
      const requestHeaders = updateRequestCookies(
        request,
        refreshResult.setCookies,
      );

      const response = NextResponse.next({
        request: {
          headers: requestHeaders,
        },
      });

      /*
       * Also send the new cookies to the browser.
       */
      for (const setCookie of refreshResult.setCookies) {
        response.headers.append("Set-Cookie", setCookie);
      }

      return response;
    }
  }

  /*
   * Refresh token is missing/expired/invalid.
   */
  return NextResponse.redirect(new URL("/login", request.url));
}

export const config = {
  matcher: [
    "/ai/:path*",
    "/dashboard/:path*",
    "/knowledge-articles/:path*",
    "/profile/:path*",
    "/tickets/:path*",
    "/users/:path*",
  ],
};
