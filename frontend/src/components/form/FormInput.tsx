import { Control, Controller, FieldPath, FieldValues } from "react-hook-form";
import Input from "../ui/Input";

type FormInputProps<T extends FieldValues> = {
  control: Control<T>;
  name: FieldPath<T>;
  placeholder?: string;
  type?: React.HTMLInputTypeAttribute;
};

export default function FormInput<T extends FieldValues>({
  control,
  name,
  placeholder,
  type = "text",
}: FormInputProps<T>) {
  return (
    <Controller
      name={name}
      control={control}
      render={({ field, fieldState }) => (
        <>
          <Input
            {...field}
            type={type}
            placeholder={placeholder}
            status={fieldState.error ? "error" : undefined}
          />
          {fieldState.error && (
            <p className="mt-1 text-sm text-red-500">
              {fieldState.error.message}
            </p>
          )}
        </>
      )}
    />
  );
}
