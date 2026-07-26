import { UserRole } from "@/types/user";

export interface UserListItem {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  phone?: string | null;
  role: UserRole;
  isActive: boolean;
  createdAtUtc: string;
}
