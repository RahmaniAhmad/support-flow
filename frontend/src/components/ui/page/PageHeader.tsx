type Props = {
  children: React.ReactNode;
};

export default function PageHeader({ children }: Props) {
  return <header className="mb-6">{children}</header>;
}
