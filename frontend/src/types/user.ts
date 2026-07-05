export interface User {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  role: UserRole;
}

export type UserRole = "Admin" | "Agent" | "Customer";
