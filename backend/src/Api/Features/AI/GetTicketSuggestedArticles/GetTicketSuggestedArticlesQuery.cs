using MediatR;

namespace Api.Features.AI.GetTicketSuggestedArticles;

public sealed record GetTicketSuggestedArticlesQuery(
    Guid TicketId)
    : IRequest<GetTicketSuggestedArticlesResponse>;