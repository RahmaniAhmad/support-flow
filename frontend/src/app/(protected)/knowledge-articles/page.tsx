import { AppPermissions } from "@/features/auth/Permissions";
import { requirePermission } from "@/features/auth/server/requirePermission";
import KnowledgeArticleList from "@/features/knowledge-articles/components/KnowledgeArticleList";

export default async function KnowledgeArticlesPage() {
  await requirePermission(AppPermissions.KnowledgeArticlesView);

  return <KnowledgeArticleList />;
}
