import { UserRole } from "@/types/user";

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
