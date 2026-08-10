import PageDescription from "@/components/ui/page/PageDescription";
import PageTitle from "@/components/ui/page/PageTitle";

type Props = {
  title?: React.ReactNode;
  description?: string;
  children: React.ReactNode;
  onSubmit: React.SubmitEventHandler;
};

export default function FormCard({
  title,
  description,
  children,
  onSubmit,
}: Props) {
  return (
    <div className="rounded-lg border border-slate-200 bg-white shadow-sm">
      <form onSubmit={onSubmit} className="p-6">
        {(title || description) && (
          <div className="mb-6">
            {title && <PageTitle>{title}</PageTitle>}

            {description && <PageDescription>{description}</PageDescription>}
          </div>
        )}

        <div className="flex flex-col gap-y-4">{children}</div>
      </form>
    </div>
  );
}
