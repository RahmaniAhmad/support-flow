export interface SuggestedArticle {
  articleId: string;

  title: string;

  content: string;

  distance: number;
}

export interface SuggestedArticlesResponse {
  results: SuggestedArticle[];
}
