"use client";

import Link from "next/link";
import { Button } from "antd";
import { ArrowLeftOutlined } from "@ant-design/icons";

type Props = {
  href: string;
  label?: string;
};

export default function BackButton({ href, label = "Back" }: Props) {
  return (
    <Link href={href}>
      <Button type="link" icon={<ArrowLeftOutlined />} className="!px-0">
        {label}
      </Button>
    </Link>
  );
}
