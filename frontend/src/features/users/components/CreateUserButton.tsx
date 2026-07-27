"use client";

import { Button } from "antd";
import { useRouter } from "next/navigation";

export default function CreateUserButton() {
  const router = useRouter();

  return (
    <Button type="primary" onClick={() => router.push("/users/create")}>
      Create User
    </Button>
  );
}
