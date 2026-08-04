import type { TableProps } from "antd";
import { ColumnsType } from "antd/es/table";

export type DataTableProps<T extends object> = {
  columns: ColumnsType<T>;

  dataSource: T[];

  loading?: boolean;

  rowKey: string | ((record: T) => string);

  pagination?: TableProps<T>["pagination"];

  skeletonRows?: number;

  onChange?: TableProps<T>["onChange"];

  onRow?: TableProps<T>["onRow"];
};
