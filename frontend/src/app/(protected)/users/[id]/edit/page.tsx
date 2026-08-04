import BackButton from "@/components/ui/navigation/BackButton";
import PageBreadcrumbs from "@/components/ui/page/PageBreadcrumbs";
import PageContent from "@/components/ui/page/PageContent";
import PageHeader from "@/components/ui/page/PageHeader";
import { AppPermissions } from "@/features/auth/Permissions";
import { requirePermission } from "@/features/auth/server/requirePermission";
import UpdateUserForm from "@/features/users/components/UpdateUserForm";
import { getUser } from "@/features/users/server/getUser";
import { notFound } from "next/navigation";

export default async function Page({
  params,
}: {
  params: Promise<{ id: string }>;
}) {
  await requirePermission(AppPermissions.UsersUpdate);

  const { id } = await params;

  const user = await getUser(id);

  if (!user) {
    notFound();
  }

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
      <UpdateUserForm user={user} userId={user.id} />
    </PageContent>
  );
}
