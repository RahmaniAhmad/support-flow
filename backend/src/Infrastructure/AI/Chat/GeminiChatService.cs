using Google.GenAI;
using Microsoft.Extensions.Options;
using Shared.AI;

namespace Infrastructure.AI.Chat;

public sealed class GeminiChatService : IChatCompletionService
{
    private readonly Client _client;

    public GeminiChatService(
        IOptions<GeminiOptions> options)
    {
        var apiKey = options.Value.ApiKey;

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new InvalidOperationException(
                "Gemini API key is not configured.");
        }

        _client = new Client(
            apiKey: apiKey);
    }


    public async Task<string> GenerateAnswerAsync(
        string question,
        string context,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(question))
        {
            throw new ArgumentException(
                "Question cannot be empty.",
                nameof(question));
        }

        var prompt = $"""
You are a helpful support assistant.

Use the knowledge base to answer the user's problem.
The user's wording may be different from the knowledge base wording.
Match the intent, not only exact words.

Only use information supported by the knowledge base.
Do not add unsupported steps.

Knowledge base:
{context}

User question:
{question}

If the knowledge base does not contain relevant information, say:
"I couldn't find this information."
""";

        try
        {
            var response =
                await _client.Models.GenerateContentAsync(
                    model: "gemini-3.8-flash",
                    contents: prompt,
                    cancellationToken: cancellationToken);


            var text = response.Text;

            if (string.IsNullOrWhiteSpace(text))
            {
                throw new InvalidOperationException(
                    "Gemini returned an empty response.");
            }

            return text;
        }
        catch (ClientError ex)
        {
            Console.WriteLine(
                $"Gemini ClientError: {ex}");

            throw;
        }
    }
}