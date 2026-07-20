import BackButton from "@/components/ui/navigation/BackButton";
import PageBreadcrumbs from "@/components/ui/page/PageBreadcrumbs";
import PageDescription from "@/components/ui/page/PageDescription";
import PageHeader from "@/components/ui/page/PageHeader";
import PageTitle from "@/components/ui/page/PageTitle";
import CreateTicketForm from "@/features/tickets/components/CreateTicketForm";

export default function CreateTicketPage() {
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
              title: "Create",
            },
          ]}
        />
      </PageHeader>
      <CreateTicketForm />;
    </div>
  );
}
