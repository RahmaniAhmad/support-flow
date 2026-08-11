export interface SemanticSearchResult {
  articleId: string;
  sourceType: string;
  content: string;
  distance: number;
}

export interface SemanticSearchResponse {
  results: SemanticSearchResult[];
}
