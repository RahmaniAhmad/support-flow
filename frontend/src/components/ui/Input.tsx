import { forwardRef, InputHTMLAttributes } from "react";
import clsx from "clsx";

type InputProps = InputHTMLAttributes<HTMLInputElement>;

const Input = forwardRef<HTMLInputElement, InputProps>(
  ({ className, ...props }, ref) => {
    return (
      <input
        ref={ref}
        className={clsx(
          "w-full rounded-lg border border-slate-300 bg-white px-3 py-2",
          "text-slate-900 placeholder:text-slate-400",
          "transition-colors",
          "focus:border-blue-600 focus:outline-none focus:ring-2 focus:ring-blue-200",
          "disabled:cursor-not-allowed disabled:bg-slate-100 disabled:opacity-70",
          className,
        )}
        {...props}
      />
    );
  },
);

Input.displayName = "Input";

export default Input;
