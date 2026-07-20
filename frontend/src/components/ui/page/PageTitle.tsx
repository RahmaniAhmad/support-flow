type Props = {
  children: React.ReactNode;
};

export default function PageTitle({ children }: Props) {
  return <h1 className="text-2xl font-bold text-slate-900">{children}</h1>;
}
