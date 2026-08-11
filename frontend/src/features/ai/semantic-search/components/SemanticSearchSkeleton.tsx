import { Card, Skeleton } from "antd";

export default function SemanticSearchSkeleton() {
  return (
    <div className="space-y-4">
      <div>
        <Skeleton.Input active size="small" style={{ width: 150 }} />

        <div className="mt-2">
          <Skeleton.Input active size="small" style={{ width: 280 }} />
        </div>
      </div>

      <div className="space-y-2!">
        {[1, 2, 3].map((item) => (
          <Card key={item} size="small">
            <Skeleton active title={{ width: "40%" }} paragraph={{ rows: 3 }} />
          </Card>
        ))}
      </div>
    </div>
  );
}
