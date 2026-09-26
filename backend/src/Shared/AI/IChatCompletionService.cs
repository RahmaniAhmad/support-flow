namespace Shared.AI;

public interface IChatCompletionService
{
    Task<string> GenerateAnswerAsync(
        string question,
        string context,
        CancellationToken cancellationToken);
}