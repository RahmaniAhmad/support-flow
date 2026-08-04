"use client";

import Button from "@/components/ui/Button";
import { useRouter } from "next/navigation";

export default function CreateUserButton() {
  const router = useRouter();

  return (
    <Button type="primary" onClick={() => router.push("/users/create")}>
      Create User
    </Button>
  );
}
