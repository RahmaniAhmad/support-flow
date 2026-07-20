type Props = {
  children: React.ReactNode;
};

export default function PageDescription({ children }: Props) {
  return <p className="mt-2 text-sm text-slate-500">{children}</p>;
}
