import type { LucideIcon } from "lucide-react";

type Props = {
  title: string;
  value: number;
  icon?: LucideIcon;
  className?: string;
};

export default function StatCard({
  title,
  value,
  icon: Icon,
  className,
}: Props) {
  return (
    <div
      className={`flex items-center justify-between gap-2 rounded-xl border border-gray-300 bg-white p-6 shadow-sm ${
        className ?? ""
      }`}
    >
      <div className="flex items-center gap-1">
        {Icon && (
          <div>
            <Icon size={24} strokeWidth={1.8} />
          </div>
        )}
        <div className="text-sm font-medium">{title}</div>
      </div>
      <div className="text-2xl font-bold">{value}</div>
    </div>
  );
}
