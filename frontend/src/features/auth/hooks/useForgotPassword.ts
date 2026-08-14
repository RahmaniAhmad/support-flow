import { useMutation } from "@tanstack/react-query";
import api from "@/lib/axios";

interface ForgotPasswordRequest {
  email: string;
}

export function useForgotPassword() {
  return useMutation({
    mutationFn: async (request: ForgotPasswordRequest) => {
      const response = await api.post("/auth/forgot-password", request);

      return response.data;
    },
  });
}
