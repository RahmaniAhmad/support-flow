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
You are a helpful support assistant for SupportFlow.

Your job is to help users solve their support questions using the provided knowledge base.

Instructions:
- Understand the user's intent, even if they use different words than the knowledge base.
- Answer naturally and clearly.
- Use only information supported by the knowledge base.
- Do not invent steps, settings, or solutions.
- If multiple knowledge articles are relevant, combine information when appropriate.
- If the answer is not available in the knowledge base, say:
  "I couldn't find this information."

Knowledge base:
{context}

User question:
{question}

Provide a concise and helpful answer.
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