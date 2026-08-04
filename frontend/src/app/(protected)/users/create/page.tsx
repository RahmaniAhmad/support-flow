import BackButton from "@/components/ui/navigation/BackButton";
import PageBreadcrumbs from "@/components/ui/page/PageBreadcrumbs";
import PageContent from "@/components/ui/page/PageContent";
import PageHeader from "@/components/ui/page/PageHeader";
import { AppPermissions } from "@/features/auth/Permissions";
import { requirePermission } from "@/features/auth/server/requirePermission";
import CreateUserForm from "@/features/users/components/CreateUserForm";

export default async function Page() {
  await requirePermission(AppPermissions.UsersCreate);
  return (
    <PageContent>
      <PageHeader>
        <BackButton fallbackHref="/users" label="Back to users" />

        <PageBreadcrumbs
          items={[
            {
              title: "Users",
            },
            {
              title: "Create",
            },
          ]}
        />
      </PageHeader>
      <CreateUserForm />
    </PageContent>
  );
}
