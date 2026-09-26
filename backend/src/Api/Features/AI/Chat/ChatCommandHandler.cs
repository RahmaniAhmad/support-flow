using MediatR;
using Shared.AI;
using Shared.Authentication;

namespace Api.Features.AI.Chat;

public sealed class ChatCommandHandler
    : IRequestHandler<ChatCommand, ChatResponse>
{
    private readonly IEmbeddingService _embeddingService;
    private readonly IVectorStore _vectorStore;
    private readonly IChatCompletionService _chatService;
    private readonly ICurrentUser _currentUser;


    public ChatCommandHandler(
        IEmbeddingService embeddingService,
        IVectorStore vectorStore,
        IChatCompletionService chatService,
        ICurrentUser currentUser)
    {
        _embeddingService = embeddingService;
        _vectorStore = vectorStore;
        _chatService = chatService;
        _currentUser = currentUser;
    }


    public async Task<ChatResponse> Handle(
        ChatCommand request,
        CancellationToken cancellationToken)
    {
        var companyId = _currentUser.CompanyId;

        if (!companyId.HasValue)
        {
            throw new InvalidOperationException(
                "Current user is not associated with a company.");
        }

        var vector =
            await _embeddingService.GenerateAsync(
                request.Question,
                cancellationToken);


        var searchResults =
            await _vectorStore.SearchAsync(
                vector,
                companyId.Value,
                limit: 5,
                cancellationToken);


        if (searchResults.Count == 0)
        {
            return new ChatResponse(
                "I couldn't find this information.",
                []);
        }


        var context = string.Join(
            "\n\n",
            searchResults.Select(x =>
            $"""
            Title:
            {x.Title}
            
            Content:
            {x.Content}
            """));

        var answer =
            await _chatService.GenerateAnswerAsync(
                request.Question,
                context,
                cancellationToken);


        var sources =
            searchResults
                .Select(x => new ChatSource(
                    x.SourceId,
                    x.SourceType,
                    x.Title,
                    x.Distance))
                .ToList();


        return new ChatResponse(
            answer,
            sources);
    }
}