"use client";

import PageTitle from "@/components/ui/page/PageTitle";
import { useKnowledgeArticles } from "../hooks/useKnowledgeArticles";
import CreateArticleButton from "./CreateArticleButton";
import Link from "next/link";
import { Modal, message } from "antd";
import { useDeleteKnowledgeArticle } from "../hooks/useDeleteKnowledgeArticle";
import { useCurrentUser } from "@/features/auth/providers/CurrentUserProvider";
import { hasPermission } from "@/features/auth/authorization";
import { AppPermissions } from "@/features/auth/Permissions";
import Button from "@/components/ui/Button";
import KnowledgeArticleSkeleton from "./KnowledgeArticleSkeleton";
import { Edit, Trash } from "lucide-react";

export default function KnowledgeArticleList() {
  const { data, isLoading } = useKnowledgeArticles();

  return (
    <div>
      <div className="flex flex-col gap-3 mb-4 sm:flex-row sm:justify-between sm:items-center">
        <PageTitle>Knowledge Articles</PageTitle>
        <div className="flex gap-3 items-center">
          <CreateArticleButton />
        </div>
      </div>

      {isLoading && <KnowledgeArticleSkeleton />}

      {!isLoading && (
        <div className="space-y-4">
          {data?.map((article) => (
            <div
              key={article.id}
              className="p-4 border border-gray-300 rounded-xl hover:bg-gray-50 flex justify-between items-start"
            >
              <Link
                href={`/knowledge-articles/${article.id}`}
                className="flex-1"
              >
                <h3 className="text-lg font-medium">{article.title}</h3>
                <div className="text-sm text-gray-500">
                  {new Date(article.createdAtUtc).toLocaleString()}
                </div>
              </Link>

              <div className="ml-4 flex items-center gap-3">
                <ActionButtons articleId={article.id} />
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}

function ActionButtons({ articleId }: { articleId: string }) {
  const currentUser = useCurrentUser();

  const canDelete = hasPermission(
    currentUser,
    AppPermissions.KnowledgeArticlesDelete,
  );
  const canUpdate = hasPermission(
    currentUser,
    AppPermissions.KnowledgeArticlesUpdate,
  );

  const deleteMutation = useDeleteKnowledgeArticle();

  const handleDelete = () => {
    Modal.confirm({
      title: "Delete article",
      content:
        "Are you sure you want to delete this article? This action cannot be undone.",
      okText: "Yes, delete",
      cancelText: "Cancel",
      okButtonProps: { danger: true },
      centered: true,
      onOk: () =>
        new Promise<void>((resolve, reject) => {
          deleteMutation.mutate(articleId, {
            onSuccess: () => {
              message.success("Article deleted.");
              resolve();
            },
            onError: () => {
              message.error("Failed to delete article.");
              reject();
            },
          });
        }),
    });
  };

  return (
    <div className="flex items-center gap-3">
      {canUpdate && (
        <Link
          href={`/knowledge-articles/${articleId}/edit`}
          className="text-sm text-blue-600"
        >
          <Edit size={16} />
        </Link>
      )}

      {canDelete && (
        <Button
          icon={<Trash size={16} />}
          type="text"
          danger
          onClick={handleDelete}
          className="text-sm"
        ></Button>
      )}
    </div>
  );
}
