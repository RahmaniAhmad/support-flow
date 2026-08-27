type FormLabelProps = {
  children: React.ReactNode;
};

export default function FormLabel({ children }: FormLabelProps) {
  return (
    <label className="mb-2 block text-sm font-medium text-slate-700">
      {children}
    </label>
  );
}
