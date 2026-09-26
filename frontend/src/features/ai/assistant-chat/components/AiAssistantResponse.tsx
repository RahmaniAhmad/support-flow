import Link from "next/link";
import { FileText, Sparkles } from "lucide-react";

import { ChatResponse } from "../types";

interface Props {
  data?: ChatResponse;
  isLoading: boolean;
}

export default function AiAssistantResponse({ data, isLoading }: Props) {
  if (isLoading) {
    return (
      <div className="rounded-lg border bg-white p-6">
        <div className="flex items-center gap-2 text-sm text-slate-600">
          <Sparkles size={16} className="animate-pulse" />
          Searching knowledge base...
        </div>
      </div>
    );
  }

  if (!data) {
    return null;
  }

  return (
    <div className="space-y-4">
      <div className="rounded-lg border bg-white p-6">
        <div className="mb-4 flex items-center gap-2">
          <Sparkles size={18} strokeWidth={1.5} fill="currentColor" />

          <h3 className="text-lg font-semibold">AI Answer</h3>
        </div>

        <p className="whitespace-pre-line text-sm leading-6 text-slate-700">
          {data.answer}
        </p>
      </div>

      {data.sources.length > 0 && (
        <div className="rounded-lg border bg-white p-6">
          <h3 className="mb-3 text-sm font-semibold text-slate-700">
            Related Knowledge Articles
          </h3>

          <div className="space-y-2">
            {data.sources.map((source) => (
              <Link
                key={source.sourceId}
                href={`/knowledge-articles/${source.sourceId}`}
                target="_blank"
                className="
                  flex
                  items-center
                  gap-3
                  rounded-md
                  border
                  px-3
                  py-2
                  text-sm
                  text-slate-700
                  transition
                  hover:bg-slate-50
                "
              >
                <FileText size={16} className="text-slate-500" />

                <span>{source.title}</span>
              </Link>
            ))}
          </div>
        </div>
      )}
    </div>
  );
}
