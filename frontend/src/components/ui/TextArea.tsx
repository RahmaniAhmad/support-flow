import clsx from "clsx";
import { Input } from "antd";

type TextAreaProps = React.ComponentProps<typeof Input.TextArea>;

export default function TextArea({ className, ...props }: TextAreaProps) {
  return (
    <Input.TextArea
      size="large"
      className={clsx("rounded-lg", className)}
      {...props}
    />
  );
}
