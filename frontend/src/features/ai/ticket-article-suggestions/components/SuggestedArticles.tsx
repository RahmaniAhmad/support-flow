"use client";

import Link from "next/link";
import { Card, Tag, Typography } from "antd";

import { useSuggestedArticles } from "../hooks/useSuggestedArticles";

const { Text } = Typography;

interface Props {
  ticketId: string;
}

export default function SuggestedArticles({ ticketId }: Props) {
  const { data, isLoading } = useSuggestedArticles(ticketId);

  if (isLoading || !data?.results.length) {
    return null;
  }

  return (
    <div className="space-y-3">
      <div>
        <h2 className="text-lg font-semibold text-slate-900">
          Suggested Articles
        </h2>

        <p className="text-sm text-slate-500">
          AI-powered suggestions based on this ticket.
        </p>
      </div>

      {data.results.map((article) => {
        const similarity = Math.max(
          0,
          Math.min(100, (1 - article.distance) * 100),
        );

        return (
          <Link
            key={article.articleId}
            href={`/knowledge-articles/${article.articleId}`}
            className="block"
          >
            <Card size="small" hoverable className="transition-shadow">
              <div className="mb-2 flex items-start justify-between gap-3">
                <Text strong>Knowledge Article</Text>

                <Tag color="blue">{similarity.toFixed(0)}% match</Tag>
              </div>

              <Typography.Paragraph ellipsis={{ rows: 3 }} className="!mb-0">
                {article.content}
              </Typography.Paragraph>
            </Card>
          </Link>
        );
      })}
    </div>
  );
}
