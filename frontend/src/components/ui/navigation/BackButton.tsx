"use client";

import { Button } from "antd";
import { ArrowLeft } from "lucide-react";
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
      className="flex! items-center gap-2 px-0!"
      onClick={handleBack}
    >
      <ArrowLeft size={18} />
      <span>{label}</span>
    </Button>
  );
}
