import BackButton from "@/components/ui/navigation/BackButton";
import PageBreadcrumbs from "@/components/ui/page/PageBreadcrumbs";
import PageHeader from "@/components/ui/page/PageHeader";
import { AppPermissions } from "@/features/auth/Permissions";
import { requirePermission } from "@/features/auth/server/requirePermission";
import TicketComments from "@/features/tickets/comments/components/TicketComments";
import TicketDetailsView from "@/features/tickets/components/TicketDetailsView";

type Props = {
  params: Promise<{
    id: string;
  }>;
};

export default async function TicketDetailsPage({ params }: Props) {
  const { id } = await params;
  await requirePermission(AppPermissions.TicketsView);

  return (
    <div>
      <PageHeader>
        <BackButton href="/tickets" label="Back to tickets" />

        <PageBreadcrumbs
          items={[
            {
              title: "Tickets",
            },
            {
              title: "Details",
            },
          ]}
        />
      </PageHeader>
      <div className="space-y-6">
        <TicketDetailsView ticketId={id} />

        <TicketComments ticketId={id} />
      </div>
    </div>
  );
}
