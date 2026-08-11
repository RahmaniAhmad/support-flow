using Infrastructure.AI.Embeddings;
using Infrastructure.AI.VectorStore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.AI;

namespace Infrastructure.AI;

public static class DependencyInjection
{
    public static IServiceCollection AddAI(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<GeminiOptions>(
            configuration.GetSection(
            GeminiOptions.SectionName));

        services.AddScoped<
            IEmbeddingService,
            GeminiEmbeddingService>();

        services.AddScoped<
            IVectorStore,
            PgVectorStore>();

        return services;
    }
}