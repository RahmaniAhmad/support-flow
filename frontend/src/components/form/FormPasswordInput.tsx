"use client";

import { Controller, Control, FieldPath, FieldValues } from "react-hook-form";
import PasswordInput from "@/components/ui/PasswordInput";

type FormPasswordInputProps<T extends FieldValues> = {
  control: Control<T>;
  name: FieldPath<T>;
  placeholder?: string;
};

export default function FormPasswordInput<T extends FieldValues>({
  control,
  name,
  placeholder,
}: FormPasswordInputProps<T>) {
  return (
    <Controller
      name={name}
      control={control}
      render={({ field, fieldState }) => (
        <>
          <PasswordInput
            {...field}
            type="password"
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
