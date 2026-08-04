import BackButton from "@/components/ui/navigation/BackButton";
import PageBreadcrumbs from "@/components/ui/page/PageBreadcrumbs";
import PageHeader from "@/components/ui/page/PageHeader";
import { AppPermissions } from "@/features/auth/Permissions";
import { requirePermission } from "@/features/auth/server/requirePermission";
import UserDetailsView from "@/features/users/components/UserDetailsView";
import { getUser } from "@/features/users/server/getUser";
import { notFound } from "next/navigation";

type Props = {
  params: Promise<{
    id: string;
  }>;
};

export default async function UserDetailsPage({ params }: Props) {
  await requirePermission(AppPermissions.UsersView);

  const { id } = await params;

  const user = await getUser(id);

  if (!user) {
    notFound();
  }

  return (
    <div>
      <PageHeader>
        <BackButton fallbackHref="/users" label="Back to users" />

        <PageBreadcrumbs
          items={[
            {
              title: "Users",
            },
            {
              title: "Details",
            },
          ]}
        />
      </PageHeader>
      <div className="space-y-6">
        <UserDetailsView user={user} />
      </div>
    </div>
  );
}
