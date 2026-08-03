import { Skeleton } from "antd";

export default function KnowledgeArticleSkeleton() {
  return (
    <div className="space-y-4">
      {Array.from({ length: 5 }).map((_, index) => (
        <div
          key={index}
          className="p-4 border border-gray-300 rounded-xl flex justify-between"
        >
          <Skeleton active title={{ width: "40%" }} paragraph={{ rows: 1 }} />

          <Skeleton.Button active size="small" />
        </div>
      ))}
    </div>
  );
}
