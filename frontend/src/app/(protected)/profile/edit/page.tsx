import BackButton from "@/components/ui/navigation/BackButton";
import PageBreadcrumbs from "@/components/ui/page/PageBreadcrumbs";
import PageHeader from "@/components/ui/page/PageHeader";
import UpdateProfileForm from "@/features/profile/components/UpdateProfileForm";

export default function ProfilePage() {
  return (
    <div>
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
      <UpdateProfileForm />
    </div>
  );
}
