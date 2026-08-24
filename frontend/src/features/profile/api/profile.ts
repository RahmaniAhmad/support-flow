import api from "@/lib/api/axios";
import { UpdateProfileRequest, UserProfile } from "../types";

export async function getProfile(): Promise<UserProfile> {
  const response = await api.get<UserProfile>("/users/profile");

  return response.data;
}

export async function updateProfile(request: UpdateProfileRequest) {
  return await api.put("/users/profile", request);
}
