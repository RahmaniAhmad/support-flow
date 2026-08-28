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
import FormError from "@/components/form/FormError";
import FormLabel from "@/components/form/FormLabel";
import { KNOWLEDGE_ARTICLES_VALIDATION } from "../constants/knowledge-articles-validation";

type Props = {
  article?: CreateArticleFormData;
  articleId?: string;
};

export default function KnowledgeArticleForm({ article, articleId }: Props) {
  const router = useRouter();
  const message = useMessage();

  const createMutation = useCreateKnowledgeArticle();
  const updateMutation = useUpdateKnowledgeArticle(articleId ?? "");

  const { control, handleSubmit } = useForm<CreateArticleFormData>({
    resolver: zodResolver(createArticleSchema),
    defaultValues: article ?? { title: "", content: "" },
  });

  function onSubmit(data: CreateArticleFormData) {
    if (articleId) {
      updateMutation.mutate(data, {
        onSuccess: () => {
          message.success("Article updated successfully.");
          router.push("/knowledge-articles");
        },
      });
    } else {
      createMutation.mutate(data, {
        onSuccess: () => {
          debugger;
          message.success("Article created successfully.");
          router.push("/knowledge-articles");
        },
      });
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
        <FormLabel>Title</FormLabel>
        <FormInput
          control={control}
          name="title"
          placeholder="How to reset password"
          maxLength={KNOWLEDGE_ARTICLES_VALIDATION.TITLE_MAX_LENGTH}
        />
      </div>
      <div>
        <FormLabel>Content</FormLabel>
        <FormTextArea
          control={control}
          name="content"
          placeholder="Article content..."
          rows={12}
          maxLength={KNOWLEDGE_ARTICLES_VALIDATION.TITLE_MAX_LENGTH}
        />
      </div>

      {createMutation.error && <FormError error={createMutation.error} />}
      {updateMutation.error && <FormError error={updateMutation.error} />}

      <Button htmlType="submit" className="w-full" isLoading={isLoading}>
        {articleId ? "Save" : "Create"}
      </Button>
    </FormCard>
  );
}
