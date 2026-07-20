import { Control, Controller, FieldPath, FieldValues } from "react-hook-form";

import TextArea from "../ui/TextArea";

type FormTextAreaProps<T extends FieldValues> = {
  control: Control<T>;
  name: FieldPath<T>;
};

export default function FormTextArea<T extends FieldValues>({
  control,
  name,
  ...props
}: FormTextAreaProps<T> & React.ComponentProps<typeof TextArea>) {
  return (
    <Controller
      name={name}
      control={control}
      render={({ field, fieldState }) => (
        <>
          <TextArea
            {...field}
            {...props}
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
