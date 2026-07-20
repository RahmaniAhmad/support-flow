import axios from "axios";
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

let isRefreshing = false;

api.interceptors.request.use((config) => {
  const csrfToken = Cookies.get(CSRF_COOKIE);

  if (csrfToken) {
    config.headers[CSRF_HEADER] = csrfToken;
  }

  return config;
});

api.interceptors.response.use(
  (response) => response,

  async (error) => {
    const originalRequest = error.config;

    if (error.response?.status !== 401 || originalRequest._retry) {
      return Promise.reject(error);
    }

    originalRequest._retry = true;

    if (!isRefreshing) {
      isRefreshing = true;

      try {
        await api.post("/auth/refresh");
        await api.get("/auth/csrf");

        isRefreshing = false;

        return api(originalRequest);
      } catch {
        isRefreshing = false;
        window.location.href = "/login";

        return Promise.reject(error);
      }
    }

    return Promise.reject(error);
  },
);

export default api;
