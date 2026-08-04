import BackButton from "@/components/ui/navigation/BackButton";
import PageBreadcrumbs from "@/components/ui/page/PageBreadcrumbs";
import PageHeader from "@/components/ui/page/PageHeader";
import { AppPermissions } from "@/features/auth/Permissions";
import { requirePermission } from "@/features/auth/server/requirePermission";
import KnowledgeArticleForm from "@/features/knowledge-articles/components/KnowledgeArticleForm";

export default async function KnowledgeArticleCreatePage() {
  await requirePermission(AppPermissions.KnowledgeArticlesCreate);

  return (
    <div>
      <PageHeader>
        <BackButton
          fallbackHref="/knowledge-articles"
          label="Back to articles"
        />

        <PageBreadcrumbs
          items={[
            {
              title: "Knowledge Articles",
            },
            {
              title: "Create",
            },
          ]}
        />
      </PageHeader>
      <KnowledgeArticleForm />
    </div>
  );
}
