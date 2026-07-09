import { forwardRef } from "react";
import clsx from "clsx";
import { Input as AntInput, InputRef } from "antd";
import type { InputProps } from "antd/es/input";

const PasswordInput = forwardRef<InputRef, InputProps>(
  ({ className, ...props }, ref) => {
    return (
      <AntInput.Password
        size="large"
        ref={ref}
        className={clsx("rounded-lg", className)}
        {...props}
      />
    );
  },
);

PasswordInput.displayName = "PasswordInput";

export default PasswordInput;
