import { Card, Skeleton } from "antd";

export function SuggestedArticlesSkeleton() {
  return (
    <Card>
      <div className="mb-4">
        <Skeleton.Input active size="small" style={{ width: 180 }} />

        <div className="mt-2">
          <Skeleton.Input active size="small" style={{ width: 300 }} />
        </div>
      </div>

      <div className="space-y-2!">
        <Card size="small">
          <Skeleton active paragraph={{ rows: 2 }} />
        </Card>

        <Card size="small">
          <Skeleton active paragraph={{ rows: 2 }} />
        </Card>
      </div>
    </Card>
  );
}
