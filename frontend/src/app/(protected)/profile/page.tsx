import ProfileView from "@/features/profile/components/ProfileView";
import { getProfile } from "@/features/profile/server/getProfile";
import { notFound } from "next/navigation";

export default async function ProfilePage() {
  const profile = await getProfile();

  if (!profile) {
    notFound();
  }

  return <ProfileView profile={profile} />;
}
