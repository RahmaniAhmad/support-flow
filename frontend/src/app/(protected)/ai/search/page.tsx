import PageContent from "@/components/ui/page/PageContent";
import SemanticSearchForm from "@/features/ai/semantic-search/components/SemanticSearchForm";
import { AppPermissions } from "@/features/auth/Permissions";
import { requirePermission } from "@/features/auth/server/requirePermission";

export default async function SearchKnowledgeArticlesPage() {
  await requirePermission(AppPermissions.AiSemanticSearch);

  return (
    <PageContent>
      <SemanticSearchForm />
    </PageContent>
  );
}
