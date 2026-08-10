"use client";

import Link from "next/link";
import { Card, Tag, Typography } from "antd";

import { useSuggestedArticles } from "../hooks/useSuggestedArticles";
import { SuggestedArticlesSkeleton } from "./SuggestedArticlesSkeleton";
import { Sparkles } from "lucide-react";

const { Text } = Typography;

interface Props {
  ticketId: string;
}

export default function SuggestedArticles({ ticketId }: Props) {
  const { data, isLoading } = useSuggestedArticles(ticketId);

  if (isLoading) {
    return <SuggestedArticlesSkeleton />;
  }

  if (!data?.results.length) {
    return null;
  }

  return (
    <div>
      <div className="mb-4">
        <div className="flex items-center gap-2">
          <Sparkles size={18} className="text-blue-500!" />

          <Typography.Title level={5} className="mb-0!">
            Suggested Articles
          </Typography.Title>
        </div>

        <p className="mt-1 text-sm text-slate-500">
          AI-powered suggestions based on this ticket.
        </p>
      </div>

      <div className="space-y-3">
        {data.results.map((article) => {
          const similarity = Math.max(
            0,
            Math.min(100, (1 - article.distance) * 100),
          );

          return (
            <Link
              key={article.articleId}
              href={`/knowledge-articles/${article.articleId}`}
              target="_blank"
              className="block"
            >
              <Card size="small" hoverable>
                <div className="mb-2 flex items-start justify-between gap-3">
                  <Text strong>Knowledge Article</Text>

                  <Tag color="blue">{similarity.toFixed(0)}% relevant</Tag>
                </div>

                <Typography.Paragraph ellipsis={{ rows: 3 }} className="!mb-0">
                  {article.content}
                </Typography.Paragraph>
              </Card>
            </Link>
          );
        })}
      </div>
    </div>
  );
}
