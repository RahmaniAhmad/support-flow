import { forwardRef } from "react";
import clsx from "clsx";
import { Input as AntInput, InputRef, InputProps as AntInputProps } from "antd";

type InputProps = AntInputProps;

const Input = forwardRef<InputRef, InputProps>(
  ({ className, ...props }, ref) => {
    return (
      <AntInput
        size="large"
        ref={ref}
        className={clsx("rounded-lg", className)}
        {...props}
      />
    );
  },
);

Input.displayName = "Input";

export default Input;
