"use client";

import { Button } from "antd";
import { ArrowLeftOutlined } from "@ant-design/icons";
import { useRouter } from "next/navigation";

type Props = {
  fallbackHref: string;
  label?: string;
};

export default function BackButton({ fallbackHref, label = "Back" }: Props) {
  const router = useRouter();

  const handleBack = () => {
    if (window.history.length > 1) {
      router.back();
    } else {
      router.push(fallbackHref);
    }
  };
  return (
    <Button
      type="link"
      icon={<ArrowLeftOutlined />}
      className="px-0!"
      onClick={handleBack}
    >
      {label}
    </Button>
  );
}
