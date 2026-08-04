"use client";

import { useRouter } from "next/navigation";
import { Modal, message } from "antd";
import { useDeleteKnowledgeArticle } from "../hooks/useDeleteKnowledgeArticle";
import { useCurrentUser } from "@/features/auth/providers/CurrentUserProvider";
import { hasPermission } from "@/features/auth/authorization";
import { AppPermissions } from "@/features/auth/Permissions";
import Button from "@/components/ui/Button";
import { KnowledgeArticleDetails } from "../types";
import { DeleteOutlined } from "@ant-design/icons";

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
      title: "Delete article",
      content:
        "Are you sure you want to delete this article? This action cannot be undone.",
      okText: "Yes, delete",
      cancelText: "Cancel",
      okButtonProps: { danger: true },
      centered: true,
      onOk: () =>
        new Promise<void>((resolve, reject) => {
          deleteMutation.mutate(article.id, {
            onSuccess: () => {
              message.success("Article deleted.");
              router.push("/knowledge-articles");
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
    <div>
      <div className="flex items-center justify-between">
        <h1 className="text-2xl font-bold">{article.title}</h1>
        {canDelete && (
          <Button
            icon={<DeleteOutlined />}
            type="text"
            danger
            onClick={handleDelete}
          ></Button>
        )}
      </div>

      <div
        className="mt-4 prose"
        dangerouslySetInnerHTML={{ __html: article.content }}
      />
    </div>
  );
}
