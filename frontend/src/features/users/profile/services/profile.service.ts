import api from "@/lib/axios";
import { UpdateProfileRequest, UserProfile } from "@/types/user";

export async function getProfile(): Promise<UserProfile> {
  const response = await api.get<UserProfile>("/users/profile");

  return response.data;
}

export async function updateProfile(request: UpdateProfileRequest) {
  return await api.put("/users/profile", request);
}
