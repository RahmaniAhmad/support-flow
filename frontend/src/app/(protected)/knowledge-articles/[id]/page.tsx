import BackButton from "@/components/ui/navigation/BackButton";
import PageBreadcrumbs from "@/components/ui/page/PageBreadcrumbs";
import PageContent from "@/components/ui/page/PageContent";
import PageHeader from "@/components/ui/page/PageHeader";
import { AppPermissions } from "@/features/auth/Permissions";
import { requirePermission } from "@/features/auth/server/requirePermission";
import KnowledgeArticleDetail from "@/features/knowledge-articles/components/KnowledgeArticleDetail";
import { getKnowledgeArticle } from "@/features/knowledge-articles/server/getKnowledgeArticle";
import { notFound } from "next/navigation";

type Props = {
  params: Promise<{
    id: string;
  }>;
};

export default async function KnowledgeArticlePage({ params }: Props) {
  await requirePermission(AppPermissions.KnowledgeArticlesView);

  const { id } = await params;

  const article = await getKnowledgeArticle(id);

  if (!article) {
    notFound();
  }

  return (
    <PageContent>
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
              title: "Details",
            },
          ]}
        />
      </PageHeader>
      <KnowledgeArticleDetail article={article} />
    </PageContent>
  );
}
