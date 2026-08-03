"use client";

import Button from "@/components/ui/Button";
import { useRouter } from "next/navigation";

export default function CreateArticleButton() {
  const router = useRouter();

  return (
    <Button
      type="primary"
      onClick={() => router.push("/knowledge-articles/create")}
    >
      Create Article
    </Button>
  );
}
