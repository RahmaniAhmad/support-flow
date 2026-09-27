"use client";

import { useState } from "react";
import { Sparkles, Send } from "lucide-react";

import FormCard from "@/components/form/FormCard";
import Button from "@/components/ui/Button";

import { useAiChat } from "../hooks/useAiChat";
import AiAssistantResponse from "./AiAssistantResponse";

export default function AiAssistant() {
  const mutation = useAiChat();
  const [question, setQuestion] = useState("");
  const [hasAsked, setHasAsked] = useState(false);

  async function handleSubmit() {
    const value = question.trim();

    if (!value) {
      return;
    }

    setHasAsked(true);

    try {
      await mutation.mutateAsync(value);
    } catch {
      // error is already handled by mutation.isError
    }
  }

  return (
    <div className="space-y-6">
      <FormCard
        onSubmit={(e) => {
          e.preventDefault();
          handleSubmit();
        }}
        title={
          <div className="flex items-center gap-2">
            <Sparkles size={18} strokeWidth={1.5} fill="currentColor" />

            <span>AI Assistant</span>
          </div>
        }
        description="Describe your problem and get answers based on the support knowledge base."
      >
        <div className="space-y-4">
          <div>
            <label className="mb-2 block text-sm font-medium text-slate-700">
              What do you need help with?
            </label>

            <textarea
              className="
                min-h-28
                w-full
                rounded-md
                border
                border-slate-200
                px-3
                py-2
                text-sm
                outline-none
                focus:border-primary
                focus:ring-1
                focus:ring-primary
              "
              value={question}
              onChange={(e) => setQuestion(e.target.value)}
              placeholder="I can't reset password"
            />
          </div>

          {mutation.isError && (
            <div className="rounded-md border border-red-200 bg-red-50 px-3 py-2 text-sm text-red-700">
              Failed to get an answer.
            </div>
          )}

          <Button
            htmlType="button"
            className="w-full"
            isLoading={mutation.isPending}
            onClick={handleSubmit}
          >
            <Send className="mr-2" size={16} />
            Ask AI Assistant
          </Button>
        </div>
      </FormCard>

      {hasAsked && (
        <AiAssistantResponse
          data={mutation.data}
          isLoading={mutation.isPending}
        />
      )}
    </div>
  );
}
