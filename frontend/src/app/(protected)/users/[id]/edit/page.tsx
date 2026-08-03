import BackButton from "@/components/ui/navigation/BackButton";
import PageBreadcrumbs from "@/components/ui/page/PageBreadcrumbs";
import PageContent from "@/components/ui/page/PageContent";
import PageHeader from "@/components/ui/page/PageHeader";
import { AppPermissions } from "@/features/auth/Permissions";
import { requirePermission } from "@/features/auth/server/requirePermission";
import UpdateUserForm from "@/features/users/components/UpdateUserForm";

export default async function Page({
  params,
}: {
  params: Promise<{ id: string }>;
}) {
  const { id } = await params;
  await requirePermission(AppPermissions.UsersUpdate);

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
              title: "Edit",
            },
          ]}
        />
      </PageHeader>
      <UpdateUserForm userId={id} />
    </PageContent>
  );
}
