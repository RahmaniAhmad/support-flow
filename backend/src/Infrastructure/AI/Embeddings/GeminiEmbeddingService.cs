using Google.GenAI;
using Google.GenAI.Types;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Pgvector;
using Shared.AI;

namespace Infrastructure.AI.Embeddings;

public sealed class GeminiEmbeddingService : IEmbeddingService
{
    private readonly Client _client;

    public GeminiEmbeddingService(IOptions<GeminiOptions> options)
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

    public async Task<Vector> GenerateAsync(
        string text,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            throw new ArgumentException(
                "Text cannot be empty.",
                nameof(text));
        }
        try
        {
            var response =
                await _client.Models.EmbedContentAsync(
                    model: "gemini-embedding-001",
                    contents: text,
                    config: new EmbedContentConfig
                    {
                        OutputDimensionality = 768
                    },
                    cancellationToken: cancellationToken);

            var values = response.Embeddings?
                .FirstOrDefault()?
                .Values;

            if (values is null || values.Count == 0)
            {
                throw new InvalidOperationException(
                    "Gemini returned an empty embedding.");
            }

            return new Vector(
                values.Select(x => (float)x).ToArray());
        }
        catch (ClientError ex)
        {
            Console.WriteLine(
                $"Gemini ClientError: {ex}");

            throw;
        }
    }
}