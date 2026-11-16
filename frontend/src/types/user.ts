export type UserRole = "SuperAdmin" | "Admin" | "Agent" | "Customer";

export interface CurrentUser {
  id: string;
  companyId: string | null;
  name: string;
  email: string;
  role: UserRole;
  permissions: string[];
}
