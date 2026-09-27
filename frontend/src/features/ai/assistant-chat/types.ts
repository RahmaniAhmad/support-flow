export interface ChatSource {
  sourceId: string;
  sourceType: string;
  title: string;
  distance: number;
}

export interface ChatResponse {
  answer: string;
  sources: ChatSource[];
}
