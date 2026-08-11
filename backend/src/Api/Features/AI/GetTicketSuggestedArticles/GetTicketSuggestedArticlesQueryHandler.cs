using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.AI;
using Shared.Authentication;
using Infrastructure.Persistence;


namespace Api.Features.AI.GetTicketSuggestedArticles;


public sealed class GetTicketSuggestedArticlesQueryHandler
    : IRequestHandler<
        GetTicketSuggestedArticlesQuery,
        GetTicketSuggestedArticlesResponse>
{

    private readonly SupportFlowDbContext _db;
    private readonly IEmbeddingService _embeddingService;
    private readonly IVectorStore _vectorStore;
    private readonly ICurrentUser _currentUser;


    public GetTicketSuggestedArticlesQueryHandler(
        SupportFlowDbContext db,
        IEmbeddingService embeddingService,
        IVectorStore vectorStore,
        ICurrentUser currentUser)
    {
        _db = db;
        _embeddingService = embeddingService;
        _vectorStore = vectorStore;
        _currentUser = currentUser;
    }


    public async Task<GetTicketSuggestedArticlesResponse> Handle(
        GetTicketSuggestedArticlesQuery request,
        CancellationToken cancellationToken)
    {

        var ticket =
            await _db.Tickets
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    x =>
                        x.Id == request.TicketId &&
                        x.CompanyId == _currentUser.CompanyId,
                    cancellationToken);


        if (ticket is null)
        {
            throw new InvalidOperationException("Ticket not found");
        }



        var queryText =
            $"""
            Ticket Subject:
            {ticket.Subject}

            Ticket Description:
            {ticket.Description}
            """;



        var vector =
            await _embeddingService.GenerateAsync(
                queryText,
                cancellationToken);



        var documents =
            await _vectorStore.SearchAsync(
                vector,
                _currentUser.CompanyId!.Value,
                5,
                cancellationToken);



        var results =
            documents
                .Select(x =>
                    new SuggestedArticleResponse(
                        x.SourceId,
                        x.Content,
                        x.Distance))
                .ToList();



        return new(results);
    }
}