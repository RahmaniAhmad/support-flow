"use client";

import { useRouter } from "next/navigation";
import { Modal, message } from "antd";
import { Trash2 } from "lucide-react";

import Button from "@/components/ui/Button";
import { useCurrentUser } from "@/features/auth/providers/CurrentUserProvider";
import { hasPermission } from "@/features/auth/authorization";
import { AppPermissions } from "@/features/auth/Permissions";

import { useDeleteKnowledgeArticle } from "../hooks/useDeleteKnowledgeArticle";
import { KnowledgeArticleDetails } from "../types";

type Props = {
  article: KnowledgeArticleDetails;
};

export default function KnowledgeArticleDetail({ article }: Props) {
  const router = useRouter();
  const currentUser = useCurrentUser();

  const deleteMutation = useDeleteKnowledgeArticle();

  const canDelete = hasPermission(
    currentUser,
    AppPermissions.KnowledgeArticlesDelete,
  );

  const handleDelete = () => {
    Modal.confirm({
      title: "Delete knowledge article?",
      content:
        "This article will be permanently deleted. This action cannot be undone.",
      okText: "Delete",
      cancelText: "Cancel",
      okButtonProps: {
        danger: true,
        loading: deleteMutation.isPending,
      },
      centered: true,

      onOk: async () => {
        try {
          await deleteMutation.mutateAsync(article.id);

          message.success("Article deleted successfully.");
          router.push("/knowledge-articles");
        } catch {
          message.error("Failed to delete the article.");
          throw new Error("Failed to delete article");
        }
      },
    });
  };

  return (
    <article className="rounded-xl bg-white p-6 shadow">
      <header className="flex flex-col gap-4 border-b border-slate-200 pb-5 sm:flex-row sm:items-start sm:justify-between">
        <div className="min-w-0">
          <h1 className="text-2xl font-bold text-slate-900">{article.title}</h1>
        </div>

        {canDelete && (
          <Button
            type="text"
            danger
            icon={<Trash2 size={16} />}
            onClick={handleDelete}
            disabled={deleteMutation.isPending}
          >
            <span className="hidden sm:inline">Delete</span>
          </Button>
        )}
      </header>

      <div
        className="prose mt-6 max-w-none"
        dangerouslySetInnerHTML={{ __html: article.content }}
      />
    </article>
  );
}
