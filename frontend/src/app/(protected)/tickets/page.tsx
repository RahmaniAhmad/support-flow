import PageContent from "@/components/ui/page/PageContent";
import { AppPermissions } from "@/features/auth/Permissions";
import { requirePermission } from "@/features/auth/server/requirePermission";
import TicketList from "@/features/tickets/components/TicketList";

export default async function TicketsPage() {
  await requirePermission(AppPermissions.TicketsView);

  return (
    <PageContent>
      <TicketList />
    </PageContent>
  );
}
