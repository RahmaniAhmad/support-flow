import { Breadcrumb } from "antd";

type Props = {
  items: {
    title: string;
  }[];
};

export default function PageBreadcrumbs({ items }: Props) {
  return <Breadcrumb items={items} />;
}
