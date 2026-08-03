import api from "@/lib/axios";
import {
  KnowledgeArticleListItem,
  CreateKnowledgeArticleRequest,
  UpdateKnowledgeArticleRequest,
} from "../types";

export async function getKnowledgeArticles() {
  const { data } = await api.get<KnowledgeArticleListItem[]>(
    "/knowledge-articles",
  );

  return data;
}

export async function createKnowledgeArticle(
  request: CreateKnowledgeArticleRequest,
) {
  const { data } = await api.post("/knowledge-articles", request);

  return data;
}

export async function updateKnowledgeArticle(
  id: string,
  request: UpdateKnowledgeArticleRequest,
) {
  await api.put(`/knowledge-articles/${id}`, request);
}

export async function deleteKnowledgeArticle(id: string) {
  await api.delete(`/knowledge-articles/${id}`);
}
