"use client";

import { Controller, Control, FieldPath, FieldValues } from "react-hook-form";
import { Form, Select } from "antd";

type Option = {
  label: string;
  value: string;
};

type Props<T extends FieldValues> = {
  control: Control<T>;
  name: FieldPath<T>;
  label?: string;
  placeholder?: string;
  options: Option[];
  loading?: boolean;
  disabled?: boolean;
};

export default function FormSelect<T extends FieldValues>({
  control,
  name,
  label,
  placeholder,
  options,
  loading,
  disabled,
}: Props<T>) {
  return (
    <Controller
      control={control}
      name={name}
      render={({ field, fieldState }) => (
        <Form.Item
          label={label}
          validateStatus={fieldState.error ? "error" : ""}
          help={fieldState.error?.message}
          className="mb-0"
        >
          <Select
            {...field}
            value={field.value || undefined}
            placeholder={placeholder}
            loading={loading}
            disabled={disabled}
            options={options}
            onChange={field.onChange}
            onBlur={field.onBlur}
            ref={field.ref}
            showSearch={{
              optionFilterProp: "label",
            }}
            allowClear
          />
        </Form.Item>
      )}
    />
  );
}
