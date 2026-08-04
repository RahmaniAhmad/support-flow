import { AppPermissions } from "@/features/auth/Permissions";
import { requirePermission } from "@/features/auth/server/requirePermission";
import KnowledgeArticleForm from "@/features/knowledge-articles/components/KnowledgeArticleForm";
import PageHeader from "@/components/ui/page/PageHeader";
import BackButton from "@/components/ui/navigation/BackButton";
import PageBreadcrumbs from "@/components/ui/page/PageBreadcrumbs";
import { notFound } from "next/navigation";
import { getKnowledgeArticle } from "@/features/knowledge-articles/server/getKnowledgeArticle";

export default async function KnowledgeArticleEditPage({
  params,
}: {
  params: Promise<{ id: string }>;
}) {
  await requirePermission(AppPermissions.KnowledgeArticlesUpdate);

  const { id } = await params;

  const article = await getKnowledgeArticle(id);

  if (!article) {
    notFound();
  }

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
              title: "Edit",
            },
          ]}
        />
      </PageHeader>
      <KnowledgeArticleForm article={article} articleId={id} />
    </div>
  );
}
