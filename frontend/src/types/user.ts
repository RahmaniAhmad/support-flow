export type UserRole = "SuperAdmin" | "Admin" | "Agent" | "Customer";

export interface CurrentUser {
  id: string;
  email: string;
  role: UserRole;
}
