export interface SuggestedArticle {
  articleId: string;

  content: string;

  distance: number;
}

export interface SuggestedArticlesResponse {
  results: SuggestedArticle[];
}
