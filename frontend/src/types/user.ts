export type UserRole = "SuperAdmin" | "Admin" | "Agent" | "Customer";

export interface CurrentUser {
  id: string;
  name: string;
  email: string;
  role: UserRole;
}
