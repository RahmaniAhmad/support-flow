import { ButtonHTMLAttributes } from "react";
import clsx from "clsx";

type ButtonVariant = "primary" | "secondary" | "danger";

interface ButtonProps extends ButtonHTMLAttributes<HTMLButtonElement> {
  variant?: ButtonVariant;
  isLoading?: boolean;
}

const variants: Record<ButtonVariant, string> = {
  primary: "bg-blue-600 text-white hover:bg-blue-700 disabled:bg-blue-400",

  secondary: "bg-slate-200 text-slate-900 hover:bg-slate-300",

  danger: "bg-red-600 text-white hover:bg-red-700",
};

export default function Button({
  variant = "primary",
  isLoading = false,
  className,
  children,
  disabled,
  ...props
}: ButtonProps) {
  return (
    <button
      disabled={disabled || isLoading}
      className={clsx(
        "inline-flex w-full items-center justify-center rounded-lg px-4 py-2",
        "font-medium transition-colors",
        "disabled:cursor-not-allowed",
        variants[variant],
        className,
      )}
      {...props}
    >
      {isLoading ? "Loading..." : children}
    </button>
  );
}
