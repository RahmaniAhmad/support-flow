import BackButton from "@/components/ui/navigation/BackButton";
import PageBreadcrumbs from "@/components/ui/page/PageBreadcrumbs";
import PageContent from "@/components/ui/page/PageContent";
import PageHeader from "@/components/ui/page/PageHeader";
import UpdateProfileForm from "@/features/profile/components/UpdateProfileForm";
import { getProfile } from "@/features/profile/server/getProfile";
import { notFound } from "next/navigation";

export default async function ProfilePage() {
  const profile = await getProfile();

  if (!profile) {
    notFound();
  }

  return (
    <PageContent>
      <PageHeader>
        <BackButton fallbackHref="/profile" label="Back to profile" />

        <PageBreadcrumbs
          items={[
            {
              title: "Profile",
            },
            {
              title: "Edit",
            },
          ]}
        />
      </PageHeader>
      <UpdateProfileForm profile={profile} />
    </PageContent>
  );
}
