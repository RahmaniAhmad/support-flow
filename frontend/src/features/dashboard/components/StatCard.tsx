type Props = {
  title: string;
  value: number;
  className?: string;
};

export default function StatCard({ title, value, className }: Props) {
  return (
    <div
      className={`rounded-xl border border-gray-300 bg-white p-6 shadow-sm ${className ?? ""}`}
    >
      <div className="flex items-start justify-between">
        <div>
          <p className="text-sm text-slate-500">{title}</p>
          <p className="mt-2 text-3xl font-bold">{value}</p>
        </div>
      </div>
    </div>
  );
}
