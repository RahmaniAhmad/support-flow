using Api.Authorization;
using Api.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.AI;
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
    private readonly ITicketAccessService _ticketAccess;


    public GetTicketSuggestedArticlesQueryHandler(
        SupportFlowDbContext db,
        IEmbeddingService embeddingService,
        IVectorStore vectorStore,
        ITicketAccessService ticketAccess)
    {
        _db = db;
        _embeddingService = embeddingService;
        _vectorStore = vectorStore;
        _ticketAccess = ticketAccess;
    }


    public async Task<GetTicketSuggestedArticlesResponse> Handle(
        GetTicketSuggestedArticlesQuery request,
        CancellationToken cancellationToken)
    {

        var ticket =
            await _ticketAccess
                .ApplyTicketAccessFilter(_db.Tickets.AsNoTracking())
                .FirstOrDefaultAsync(
                    x => x.Id == request.TicketId,
                    cancellationToken);


        if (ticket is null)
        {
            throw new NotFoundException(
                "Ticket was not found.",
                "TICKET_NOT_FOUND");
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
                ticket.CompanyId,
                5,
                cancellationToken);



        var results =
            documents
                .Select(x =>
                    new SuggestedArticleResponse(
                        x.SourceId,
                        x.Title,
                        x.Content,
                        x.Distance))
                .ToList();



        return new(results);
    }
}