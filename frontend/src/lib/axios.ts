import axios, { AxiosError, InternalAxiosRequestConfig } from "axios";
import Cookies from "js-cookie";

const CSRF_HEADER = "X-CSRF-TOKEN";
const CSRF_COOKIE = "XSRF-TOKEN";

const api = axios.create({
  baseURL: process.env.NEXT_PUBLIC_API_URL,
  withCredentials: true,
  headers: {
    "Content-Type": "application/json",
  },
});

let refreshPromise: Promise<void> | null = null;

api.interceptors.request.use((config) => {
  const csrfToken = Cookies.get(CSRF_COOKIE);

  if (csrfToken) {
    config.headers[CSRF_HEADER] = csrfToken;
  }

  return config;
});

api.interceptors.response.use(
  (response) => response,

  async (error: AxiosError) => {
    const originalRequest = error.config as InternalAxiosRequestConfig & {
      _retry?: boolean;
    };

    if (error.response?.status !== 401 || originalRequest?._retry) {
      return Promise.reject(error);
    }

    originalRequest._retry = true;

    try {
      if (!refreshPromise) {
        refreshPromise = api
          .post("/auth/refresh")
          .then(async () => {
            await api.get("/auth/csrf");
          })
          .finally(() => {
            refreshPromise = null;
          });
      }

      await refreshPromise;

      return api(originalRequest);
    } catch (refreshError) {
      refreshPromise = null;

      window.location.href = "/login";

      return Promise.reject(refreshError);
    }
  },
);

export default api;
