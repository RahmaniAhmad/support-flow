export type UserRole = "Admin" | "Agent" | "Customer";

export interface User {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  phone?: string | null;
  role: UserRole;
  isActive: boolean;
  createdAtUtc: string;
}

export interface UserProfile {
  email: string;
  firstName: string;
  lastName: string;
  phone?: string | null;
  role: UserRole;
  companyName?: string | null;
}

export interface UpdateProfileRequest {
  firstName: string;
  lastName: string;
  phone?: string | null;
}
