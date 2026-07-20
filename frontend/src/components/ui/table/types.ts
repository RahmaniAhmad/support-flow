import type { TableProps } from "antd";

export type DataTableProps<T extends object> = {
  columns: TableProps<T>["columns"];

  dataSource: T[];

  loading?: boolean;

  rowKey: string | ((record: T) => string);

  pagination?: TableProps<T>["pagination"];

  onChange?: TableProps<T>["onChange"];
};
