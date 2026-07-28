import PageTitle from "@/components/ui/page/PageTitle";
import PageDescription from "@/components/ui/page/PageDescription";

type Props = {
  title?: string;
  description?: string;
  children: React.ReactNode;
  onSubmit: React.FormEventHandler<HTMLFormElement>;
};

export default function FormCard({
  title,
  description,
  children,
  onSubmit,
}: Props) {
  return (
    <form
      noValidate
      onSubmit={onSubmit}
      className="
        w-full
        max-w-3xl
        rounded-xl
        bg-white
        p-6
        shadow
      "
    >
      <div className="mb-6 mt-3">
        {title && <PageTitle>{title}</PageTitle>}

        {description && <PageDescription>{description}</PageDescription>}
      </div>

      <div className="flex flex-col gap-y-4">{children}</div>
    </form>
  );
}
