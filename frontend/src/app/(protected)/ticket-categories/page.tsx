import PageContent from "@/components/ui/page/PageContent";
import { AppPermissions } from "@/features/auth/Permissions";
import { requirePermission } from "@/features/auth/server/requirePermission";
import TicketCategoryList from "@/features/ticket-categories/components/TicketCategoryList";

export default async function TicketCategoriesPage() {
  await requirePermission(AppPermissions.TicketCategoriesView);

  return (
    <PageContent>
      <TicketCategoryList />
    </PageContent>
  );
}
