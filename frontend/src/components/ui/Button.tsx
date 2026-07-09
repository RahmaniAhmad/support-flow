import { Button as AntButton } from "antd";
import type { ButtonProps as AntButtonProps } from "antd";

interface ButtonProps extends AntButtonProps {
  isLoading?: boolean;
}

export default function Button({
  type = "primary",
  isLoading = false,
  children,
  disabled,
  ...props
}: ButtonProps) {
  return (
    <AntButton
      size="large"
      type={type}
      loading={isLoading}
      disabled={disabled || isLoading}
      {...props}
    >
      {children}
    </AntButton>
  );
}
