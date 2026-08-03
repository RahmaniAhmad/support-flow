import serverApi from "@/lib/server-api";

export async function getKnowledgeArticle(id: string) {
  try {
    const response = await serverApi.get(`/knowledge-articles/${id}`);

    return response.data;
  } catch {
    return null;
  }
}
