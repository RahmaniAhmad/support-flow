"use client";

import { useRouter } from "next/navigation";
import { zodResolver } from "@hookform/resolvers/zod";
import { useForm } from "react-hook-form";
import Button from "@/components/ui/Button";
import FormCard from "@/components/form/FormCard";
import FormInput from "@/components/form/FormInput";
import FormTextArea from "@/components/form/FormTextArea";
import {
  createArticleSchema,
  CreateArticleFormData,
} from "../schemas/create-article.schema";
import { useCreateKnowledgeArticle } from "../hooks/useCreateKnowledgeArticle";
import { useUpdateKnowledgeArticle } from "../hooks/useUpdateKnowledgeArticle";
import { useMessage } from "@/app/providers/MessageProvider";

type Props = {
  initialValues?: CreateArticleFormData;
  articleId?: string;
};

export default function KnowledgeArticleForm({
  initialValues,
  articleId,
}: Props) {
  const router = useRouter();
  const message = useMessage();

  const createMutation = useCreateKnowledgeArticle();
  const updateMutation = useUpdateKnowledgeArticle(articleId);

  const { control, handleSubmit } = useForm<CreateArticleFormData>({
    resolver: zodResolver(createArticleSchema),
    defaultValues: initialValues ?? { title: "", content: "" },
  });

  async function onSubmit(data: CreateArticleFormData) {
    try {
      if (articleId) {
        await updateMutation.mutateAsync(data);
        message.success("Article updated successfully.");
        router.push("/knowledge-articles");
      } else {
        await createMutation.mutateAsync(data);
        message.success("Article created successfully.");
        router.push("/knowledge-articles");
      }
    } catch {
      // handled via mutation error state
    }
  }

  const isLoading = createMutation.isPending || updateMutation.isPending;

  return (
    <FormCard
      title={articleId ? "Edit Article" : "Create Article"}
      description={
        articleId ? "Update the article." : "Create a new knowledge article."
      }
      onSubmit={handleSubmit(onSubmit)}
    >
      <div>
        <label className="mb-2 block text-sm font-medium text-slate-700">
          Title
        </label>
        <FormInput
          control={control}
          name="title"
          placeholder="How to reset password"
        />
      </div>

      <div>
        <label className="mb-2 block text-sm font-medium text-slate-700">
          Content
        </label>
        <FormTextArea
          control={control}
          name="content"
          placeholder="Article content..."
          rows={12}
        />
      </div>

      <Button htmlType="submit" className="w-full" isLoading={isLoading}>
        {articleId ? "Save" : "Create"}
      </Button>
    </FormCard>
  );
}
