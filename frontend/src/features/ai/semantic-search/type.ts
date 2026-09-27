export interface SemanticSearchResult {
  articleId: string;
  sourceType: string;
  title: string;
  content: string;
  distance: number;
}

export interface SemanticSearchResponse {
  results: SemanticSearchResult[];
}
