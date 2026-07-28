import BackButton from "@/components/ui/navigation/BackButton";
import PageBreadcrumbs from "@/components/ui/page/PageBreadcrumbs";
import PageHeader from "@/components/ui/page/PageHeader";
import { AppPermissions } from "@/features/auth/Permissions";
import { requirePermission } from "@/features/auth/server/requirePermission";
import UserDetailsView from "@/features/users/components/UserDetailsView";

type Props = {
  params: Promise<{
    id: string;
  }>;
};

export default async function UserDetailsPage({ params }: Props) {
  const { id } = await params;
  await requirePermission(AppPermissions.UsersView);

  return (
    <div>
      <PageHeader>
        <BackButton href="/users" label="Back to users" />

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
        <UserDetailsView userId={id} />
      </div>
    </div>
  );
}
