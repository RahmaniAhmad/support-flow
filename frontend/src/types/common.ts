export type SortDirection = "asc" | "desc";
export interface PagedResponse<T> {
  items: T[];
  page: number;
  pageSize: number;
  totalCount: number;
}

export interface ApiError {
  message: string;
  errors?: string[];
}
