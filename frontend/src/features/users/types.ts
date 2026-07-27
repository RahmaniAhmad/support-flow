import { SortDirection } from "@/types/common";
import { UserRole } from "@/types/user";

export interface UserListItem {
  id: string;
  companyName?: string;
  firstName: string;
  lastName: string;
  email: string;
  phone?: string | null;
  role: UserRole;
  isActive: boolean;
  createdAtUtc: string;
}

export interface UserDetails {
  id: string;
  email: string;
  firstName: string;
  lastName: string;
  phone?: string | null;
  role: UserRole;
  isActive: boolean;
  companyName?: string | null;
  createdAtUtc: string;
}

export interface UserListFilters {
  page?: number;
  pageSize?: number;
  search?: string;
  role?: UserRole;
  isActive?: boolean;
  sortBy?: string;
  sortDirection?: SortDirection;
}

export interface CreateUserRequest {
  email: string;
  password: string;
  firstName: string;
  lastName: string;
  phone?: string;
  role: UserRole;
}
