import BackButton from "@/components/ui/navigation/BackButton";
import PageBreadcrumbs from "@/components/ui/page/PageBreadcrumbs";
import PageContent from "@/components/ui/page/PageContent";
import PageHeader from "@/components/ui/page/PageHeader";
import SuggestedArticles from "@/features/ai/ticket-article-suggestions/components/SuggestedArticles";
import { AppPermissions } from "@/features/auth/Permissions";
import { requirePermission } from "@/features/auth/server/requirePermission";
import TicketComments from "@/features/tickets/comments/components/TicketComments";
import TicketDetailsView from "@/features/tickets/components/TicketDetailsView";
import { getTicket } from "@/features/tickets/server/getTicket";
import { notFound } from "next/navigation";

type Props = {
  params: Promise<{
    id: string;
  }>;
};

export default async function TicketDetailsPage({ params }: Props) {
  await requirePermission(AppPermissions.TicketsView);

  const { id } = await params;

  const ticket = await getTicket(id);

  if (!ticket) {
    notFound();
  }

  return (
    <PageContent>
      <PageHeader>
        <BackButton fallbackHref="/tickets" label="Back" />

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
      <div className="grid gap-4 lg:grid-cols-[minmax(0,1fr)_360px]">
        <div className="space-y-4">
          <TicketDetailsView ticket={ticket} />

          <TicketComments ticketId={id} />
        </div>

        <aside className="lg:sticky lg:top-4 lg:self-start">
          <SuggestedArticles ticketId={ticket.id} />
        </aside>
      </div>
    </PageContent>
  );
}
