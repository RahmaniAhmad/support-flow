type Props = {
  children: React.ReactNode;
};

export default function PageHeader({ children }: Props) {
  return <header className="mb-6 flex items-center gap-2">{children}</header>;
}
