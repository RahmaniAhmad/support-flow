export type KnowledgeArticleListItem = {
  id: string;
  title: string;
  createdAtUtc: string;
};

export type KnowledgeArticleDetails = {
  id: string;
  title: string;
  content: string;
  createdAtUtc: string;
};

export type CreateKnowledgeArticleRequest = {
  title: string;
  content: string;
};

export type UpdateKnowledgeArticleRequest = CreateKnowledgeArticleRequest;
