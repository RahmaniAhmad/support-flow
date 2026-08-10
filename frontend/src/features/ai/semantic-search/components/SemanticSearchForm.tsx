"use client";

import Link from "next/link";
import { AxiosError } from "axios";
import { zodResolver } from "@hookform/resolvers/zod";
import { useForm } from "react-hook-form";
import { GeminiFilled } from "@ant-design/icons";

import FormCard from "@/components/form/FormCard";
import FormInput from "@/components/form/FormInput";
import Button from "@/components/ui/Button";

import { useSemanticSearch } from "../hooks/useSemanticSearch";
import {
  SemanticSearchFormData,
  semanticSearchSchema,
} from "../schemas/semantic-search.schema";

export default function SemanticSearchForm() {
  const mutation = useSemanticSearch();

  const { control, handleSubmit } = useForm({
    resolver: zodResolver(semanticSearchSchema),
    defaultValues: {
      query: "",
    },
  });

  async function onSubmit(data: SemanticSearchFormData) {
    await mutation.mutateAsync({
      query: data.query,
      limit: 5,
    });
  }

  const error =
    mutation.error instanceof AxiosError
      ? (mutation.error.response?.data?.message ??
        "Failed to perform semantic search.")
      : mutation.isError
        ? "Failed to perform semantic search."
        : null;

  return (
    <div className="space-y-6">
      <FormCard
        onSubmit={handleSubmit(onSubmit)}
        title={
          <span className="flex items-center gap-2">
            <GeminiFilled />
            AI Semantic Search
          </span>
        }
        description="Search your knowledge base using AI. Ask your question naturally and find the most relevant articles."
      >
        <div>
          <label
            htmlFor="query"
            className="mb-2 block text-sm font-medium text-slate-700"
          >
            Your question
          </label>

          <FormInput
            control={control}
            name="query"
            placeholder="How do I configure SMTP email?"
          />
        </div>

        {error && (
          <div className="rounded-md border border-red-200 bg-red-50 px-3 py-2 text-sm text-red-700">
            {error}
          </div>
        )}

        <Button
          htmlType="submit"
          className="w-full"
          isLoading={mutation.isPending}
        >
          <GeminiFilled className="mr-2" />
          Search with AI
        </Button>
      </FormCard>

      {mutation.data && mutation.data.results.length === 0 && (
        <div className="rounded-lg border border-slate-200 bg-white p-6 text-center text-sm text-slate-500">
          No relevant articles were found.
        </div>
      )}

      {mutation.data && mutation.data.results.length > 0 && (
        <div className="space-y-4">
          <div className="flex items-center gap-2">
            <GeminiFilled className="text-blue-600" />

            <h2 className="text-lg font-semibold text-slate-900">
              AI Search Results
            </h2>
          </div>

          {mutation.data.results.map((result) => {
            const similarity = Math.max(
              0,
              Math.min(100, (1 - result.distance) * 100),
            );

            return (
              <Link
                key={result.articleId}
                href={`/knowledge-articles/${result.articleId}`}
                className="block rounded-lg border border-slate-200 bg-white p-5 transition hover:border-blue-400 hover:bg-blue-50"
              >
                <div className="mb-3 flex items-start justify-between gap-4">
                  <span className="shrink-0 rounded-full bg-blue-50 px-2.5 py-1 text-xs font-medium text-blue-700">
                    {similarity.toFixed(0)}% match
                  </span>
                </div>

                <p className="text-sm leading-6 text-slate-600">
                  {result.content.substring(0, 250)}
                  {result.content.length > 250 && "..."}
                </p>
              </Link>
            );
          })}
        </div>
      )}
    </div>
  );
}
