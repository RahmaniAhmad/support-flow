"use client";

import Link from "next/link";
import { Card, Tag, Typography } from "antd";

import SemanticSearchSkeleton from "./SemanticSearchSkeleton";
import { SemanticSearchResponse } from "../type";
import { Sparkles } from "lucide-react";

interface Props {
  data?: SemanticSearchResponse;
  isLoading: boolean;
}

export default function SemanticSearchResults({ data, isLoading }: Props) {
  if (isLoading) {
    return <SemanticSearchSkeleton />;
  }

  if (!data) {
    return null;
  }

  if (!data.results.length) {
    return (
      <Card>
        <div className="py-6 text-center">
          <Typography.Text className="text-slate-500">
            No relevant articles were found.
          </Typography.Text>

          <p className="mt-1 text-sm text-slate-400">
            Try using different keywords or describing your question
            differently.
          </p>
        </div>
      </Card>
    );
  }

  return (
    <div className="space-y-4">
      <div>
        <div className="flex items-center gap-2">
          <Sparkles size={18} />

          <Typography.Title level={5} className="mb-0!">
            Search Results
          </Typography.Title>
        </div>

        <p className="mt-1 text-sm text-slate-500">
          {data.results.length} relevant{" "}
          {data.results.length === 1 ? "article" : "articles"} found.
        </p>
      </div>

      <div className="space-y-3">
        {data.results.map((result) => {
          const similarity = Math.max(
            0,
            Math.min(100, (1 - result.distance) * 100),
          );

          return (
            <Link
              key={result.articleId}
              href={`/knowledge-articles/${result.articleId}`}
              className="block"
            >
              <Card size="small" hoverable className="transition-shadow">
                <div className="mb-2 flex items-start justify-between gap-3">
                  <Typography.Text strong>Knowledge Article</Typography.Text>

                  <Tag color="blue">{similarity.toFixed(0)}% relevant</Tag>
                </div>

                <Typography.Paragraph ellipsis={{ rows: 3 }} className="!mb-2">
                  {result.content}
                </Typography.Paragraph>

                <Typography.Text type="secondary">
                  View article →
                </Typography.Text>
              </Card>
            </Link>
          );
        })}
      </div>
    </div>
  );
}
