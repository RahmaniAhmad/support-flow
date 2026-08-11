import BackButton from "@/components/ui/navigation/BackButton";
import PageBreadcrumbs from "@/components/ui/page/PageBreadcrumbs";
import PageContent from "@/components/ui/page/PageContent";
import PageHeader from "@/components/ui/page/PageHeader";
import { AppPermissions } from "@/features/auth/Permissions";
import { requirePermission } from "@/features/auth/server/requirePermission";
import CreateTicketForm from "@/features/tickets/components/CreateTicketForm";

export default async function CreateTicketPage() {
  await requirePermission(AppPermissions.TicketsView);

  return (
    <PageContent>
      <PageHeader>
        <BackButton fallbackHref="/tickets" label="Back to tickets" />

        <PageBreadcrumbs
          items={[
            {
              title: "Tickets",
            },
            {
              title: "Create",
            },
          ]}
        />
      </PageHeader>
      <CreateTicketForm />
    </PageContent>
  );
}
