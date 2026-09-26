import { useMutation } from "@tanstack/react-query";
import { sendAiMessage } from "../api";

export function useAiChat() {
  return useMutation({
    mutationFn: sendAiMessage,
  });
}
