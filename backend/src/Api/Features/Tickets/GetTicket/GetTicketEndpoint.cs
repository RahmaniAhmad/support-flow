using MediatR;
using Shared.Authentication;

namespace Api.Features.Tickets.GetTicket;

public static class GetTicketEndpoint
{
    public static IEndpointRouteBuilder MapGetTicket(
        this IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/tickets/{id:guid}",
            GetTicketAsync)
            .RequireAuthorization();

        return app;
    }

    private static async Task<IResult> GetTicketAsync(
        Guid id,
        ICurrentUser currentUser,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var ticket = await sender.Send(
            new GetTicketQuery(id, currentUser.CompanyId),
            cancellationToken);

        return ticket is null
            ? Results.NotFound()
            : Results.Ok(ticket);
    }
}