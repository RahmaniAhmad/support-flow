import api from "@/lib/api/axios";
import { ChatResponse } from "./types";

export async function sendAiMessage(question: string): Promise<ChatResponse> {
  const response = await api.post<ChatResponse>("/api/ai/chat", {
    question,
  });

  return response.data;
}
