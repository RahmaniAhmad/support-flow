namespace Api.Features.AI.Chat;

public sealed record ChatResponse(
    string Answer,
    IReadOnlyList<ChatSource> Sources);


public sealed record ChatSource(
    Guid SourceId,
    string SourceType,
    double Distance);