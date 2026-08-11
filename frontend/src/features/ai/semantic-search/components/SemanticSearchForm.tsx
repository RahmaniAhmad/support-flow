"use client";

import { useState } from "react";
import { AxiosError } from "axios";
import { zodResolver } from "@hookform/resolvers/zod";
import { useForm } from "react-hook-form";

import FormCard from "@/components/form/FormCard";
import FormInput from "@/components/form/FormInput";
import Button from "@/components/ui/Button";

import { useSemanticSearch } from "../hooks/useSemanticSearch";
import {
  SemanticSearchFormData,
  semanticSearchSchema,
} from "../schemas/semantic-search.schema";
import SemanticSearchResults from "./SemanticSearchResults";
import { Search, Sparkles } from "lucide-react";

export default function SemanticSearchForm() {
  const mutation = useSemanticSearch();
  const [hasSearched, setHasSearched] = useState(false);

  const { control, handleSubmit } = useForm<SemanticSearchFormData>({
    resolver: zodResolver(semanticSearchSchema),
    defaultValues: {
      query: "",
    },
  });

  async function onSubmit(data: SemanticSearchFormData) {
    setHasSearched(true);

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
          <div className="flex items-center gap-2">
            <Sparkles strokeWidth={1.5} fill="currentColor" />
            <span>Search Knowledge Base</span>
          </div>
        }
        description="Find relevant knowledge articles using natural language."
      >
        <div className="space-y-4">
          <div>
            <label className="mb-2 block text-sm font-medium text-slate-700">
              What do you need help with?
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
            <Search className="mr-2" size={16} />
            Search Knowledge Base
          </Button>
        </div>
      </FormCard>

      {hasSearched && (
        <SemanticSearchResults
          data={mutation.data}
          isLoading={mutation.isPending}
        />
      )}
    </div>
  );
}
